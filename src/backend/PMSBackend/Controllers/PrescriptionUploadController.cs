using ImageMagick;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.PrescriptionUpload;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PatientFolders;
using PMSBackend.Application.Queries.PrescriptionUpload;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all prescription upload endpoints
    public class PrescriptionUploadController : ControllerBase
    {
        public readonly IMediator _mediator;
        // private readonly ILogger _logger;
        public PrescriptionUploadController(IMediator mediator)//, ILogger logger)
        {
            _mediator = mediator;
            // _logger = logger;
        }

        [HttpPost("file-upload")]
        [ProducesDefaultResponseType(typeof(PrescriptionUploadDTO))]
        public async Task<ActionResult> FileUploadAsync([FromForm] List<IFormFile> files, [FromForm] InsertPrescriptionUploadCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var checkResult = CheckIfNull(files, command);
                if (checkResult is not null)
                    return checkResult;

                List<IFormFile> imageFiles = new List<IFormFile>();
                List<IFormFile> pdfFiles = new List<IFormFile>();
                var checkMatched = CheckFileFormatMatched(files, imageFiles, pdfFiles);
                if (checkMatched is not null)
                    return checkMatched;

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string newUniquePrescriptionCode = await GetFileNewSequence(command);
                int fileCount = 0;

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    fileCount++;

                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    command.FileExtension = extension;
                    var isImage = file.ContentType.StartsWith("image/") || new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }.Contains(extension);
                    var isPdf = file.ContentType == "application/pdf" || extension == ".pdf";

                    var baseFileName = $"File_{fileCount}{extension}";
                    var savePath = string.Empty;

                    if (isImage)
                    {
                        command = await SaveImageInMultipleSizesAsync(file, fileCount, uploadsFolder, command);
                    }
                    else if (isPdf)
                    {
                        // Save PDF to pdf folder
                        command = await SavePDFAndConvertIntoImageInMultipleSizesAsync(file, fileCount, uploadsFolder, command);
                    }
                    else
                    {
                        // Skip unsupported files (optional)
                        continue;
                    }
                }                 

            // fileCount = command.FileCount;
            command.FileCount = files.Count;
                PrescriptionUploadDTO result = await _mediator.Send(command, cancellationToken);
                var fileInfo = new { Id = result.Id, PrescriptionCode = result.PrescriptionCode ?? newUniquePrescriptionCode, FileName = command.FileName, FilePath = command.FilePath, IsRewardUpdated= result.IsRewardUpdated, RewardTitle= result.RewardTitle, TotalRewardPoints=result.TotalRewardPoints, RewardMessage=result.RewardMessage };

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status201Created, new ApiResponseResult
                    {
                        Data = fileInfo,
                        StatusCode = StatusCodes.Status201Created,
                        Status = "Success",
                        Message = $"Your prescription (RX) with {command.FileCount} files saved successfully"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Prescription (RX) upload failed"
                    });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with the system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GetFileNewSequence(InsertPrescriptionUploadCommand command)
        {
            PrescriptionUploadSequenceGenerateCommand newSeqId = new PrescriptionUploadSequenceGenerateCommand() { SeqNo = "" };
            var newUniquePrescriptionCode = await _mediator.Send(newSeqId);
            command.SeqNo = newUniquePrescriptionCode;
            return newUniquePrescriptionCode;
        }

        private ActionResult? CheckIfNull(List<IFormFile> files, InsertPrescriptionUploadCommand command)
        {
            if (files == null || files.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "No file uploaded"
                });
            }
            return null;
        }

        private ActionResult? CheckFileFormatMatched(List<IFormFile> allFiles, List<IFormFile> imageFiles, List<IFormFile> pdfFiles)
        {
            bool hasPdf = false;
            bool hasImage = false;
            var allowedImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            foreach (var file in allFiles)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (extension == ".pdf")
                {
                    hasPdf = true;
                    pdfFiles?.Add(file);
                }
                if (allowedImageTypes.Contains(extension))
                {
                    hasImage = true;
                    imageFiles?.Add(file);
                }
                //if (hasPdf && hasImage)
                //    break;
            }
            if (!hasImage && !hasPdf)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "File format does not match. Please select only image or pdf"
                });
            }
            return null;
        }

        private async Task<InsertPrescriptionUploadCommand> SaveImageInMultipleSizesAsync(IFormFile file, int fileCount, string outputFolder, InsertPrescriptionUploadCommand command)
        {
            using var stream = file.OpenReadStream();
            using var image = await Image.LoadAsync(stream);

            var sizes = new Dictionary<string, (int width, int height)>
            {
                { "large", (656, 850) },
                { "original", await GetImageDimensionsAsync(file) },
                { "thumbnail", (300, 200) },
            };
            foreach (var size in sizes)
            {
                Image resized;

                if (size.Key == "thumbnail")
                {
                    resized = image.Clone(x => x
                        .Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Size = new Size(300, 200),
                            Sampler = KnownResamplers.Lanczos3
                        })
                        .GaussianSharpen(0.6f)
                    );
                }
                else
                {
                    resized = image.Clone(x => x
                        .Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(size.Value.width, size.Value.height),
                            Sampler = KnownResamplers.Lanczos3
                        })
                    );
                }
                if ((fileCount == 1 && size.Key == "thumbnail") || size.Key == "large" || size.Key == "original")
                {
                    var outputPath = Path.Combine(outputFolder, $"{command.SeqNo}_{fileCount}_{size.Key}.jpg");
                    using (resized)
                    {
                        // Save or use the resized image here
                        await resized.SaveAsync(outputPath, new JpegEncoder
                        {
                            Quality = size.Key == "thumbnail" ? 85 : 90,
                        });
                    }
                    if (fileCount == 1 && size.Key == "thumbnail")
                    {
                        var outputForViewPath = Path.Combine("files", $"{command.SeqNo}_{fileCount}_{size.Key}.jpg");
                        command.FilePath = outputForViewPath;
                        Console.WriteLine("File Upload line 236: ",outputFolder);
                    }
                }
            }
            command.FileCount = fileCount;
            return command;
        }
        private async Task<InsertPrescriptionUploadCommand> SavePDFAndConvertIntoImageInMultipleSizesAsync(IFormFile file, int fileCount, string outputFolder, InsertPrescriptionUploadCommand command)
        {
            var pdfOutputPath = Path.Combine(outputFolder, $"{command.SeqNo}_{fileCount}_original.pdf");
            using (var stream = new FileStream(pdfOutputPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var sizes = new Dictionary<string, (int width, int height)>
            {
                //{ "large", (656, 850) },
                { "thumbnail", (300, 200) },
               // { "original", await GetImageDimensionsAsync(command.File) },
            };
            foreach (var size in sizes)
            {
                var settings = new MagickReadSettings
                {
                    Density = new Density(size.Value.width, size.Value.height) // DPI for smaller image
                };
                if (fileCount == 1 && size.Key == "thumbnail")
                {
                    using var images = new MagickImageCollection();
                    images.Read(pdfOutputPath, settings);
                    var outputPath = Path.Combine(outputFolder, $"{command.SeqNo}_{fileCount}_{size.Key}.jpg");
                    using var firstPage = (MagickImage)images[0]; // Get first page only
                    firstPage.Resize((uint)size.Value.width, 0); // Resize width, keep aspect ratio
                    firstPage.Format = MagickFormat.Jpeg;
                    firstPage.Quality = 70; // Lower quality = smaller size
                    firstPage.Write(outputPath);
                    var outputForViewPath = Path.Combine("files", $"{command.SeqNo}_{fileCount}_{size.Key}.jpg");
                    command.FilePath = outputForViewPath;
                    Console.WriteLine("File Upload line 274: ", outputForViewPath);
                }
            }
            command.FileCount = fileCount;
            await Task.CompletedTask;
            return command;
        }
        private async Task<(int Width, int Height)> GetImageDimensionsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            using var stream = file.OpenReadStream();
            using var image = await Image.LoadAsync<SixLabors.ImageSharp.PixelFormats.Rgba32>(stream); // Load with pixel format for accurate dimension

            return (image.Width, image.Height);
        }

       
        [HttpPut("update-smartrx-request/{Id:long}")]
        [ProducesDefaultResponseType(typeof(PrescriptionUploadDTO))]
        public async Task<ActionResult> UploadedFileUpdateAsync(long id, [FromBody] EditForSmartRxRequestCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id != command.PrescriptionId)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "File id is not correct"
                    });
                }
                PrescriptionUploadDTO result = await _mediator.Send(command, cancellationToken);

                if (result != null)
                {
                    var fileInfo = new { Id = result.Id, PrescriptionCode = result.PrescriptionCode, FileName = result.FileName, UniqueFileName = result.FileName, FilePath = result.FilePath, RequestedForSmartRx = result.IsSmartRxRequested, IsRewardUpdated = result.IsRewardUpdated, RewardTitle = result.RewardTitle, TotalRewardPoints = result.TotalRewardPoints, RewardMessage = result.RewardMessage };
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = fileInfo,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Your prescription (RX) file is requested to upgrade into SmartRx. You will be notified within 24 hours"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Prescription (RX) file request for SmartRx is failed."
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // To move, tag, and change other information use this api.
        [HttpPut("update-uploaded-file/{Id:long}")]
        [ProducesDefaultResponseType(typeof(PrescriptionUploadDTO))]
        public async Task<ActionResult> UploadedFileUpdateAsync(long id, [FromBody] EditUploadedPrescriptionCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id != command.PrescriptionId)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "File is not correct. Please select the right file."
                    });
                }
                PrescriptionUploadDTO result = await _mediator.Send(command, cancellationToken);
                if (result != null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }

                    var fileInfo = new
                    {
                        Id = result.Id,
                        UserId = result.UserId,
                        FolderId = result.FolderId,
                        PrescriptionCode = result.PrescriptionCode,
                        RequestedForSmartRx = result.IsSmartRxRequested,
                        IsExistingPatient = result.IsExistingPatient,
                        PatientId = result.PatientId,
                        HasExistingRelative = result.HasExistingRelative,
                        RelativePatientIds = result.RelativePatientIds,
                        IsSmartRxRequested = result.IsSmartRxRequested,
                        IsLocked = result.IsLocked,
                        LockedBy = result.LockedBy,
                        LockedDate = result?.LockedDate,
                        IsReported = result?.IsReported,
                        ReportBy = result?.ReportBy,
                        ReportDate = result?.ReportDate,
                        ReportDetails = result?.ReportDetails,
                        ReportReason = result?.ReportReason,
                        IsRecommended = result?.IsRecommended,
                        RecommendedBy = result?.RecommendedBy,
                        RecommendedDate = result?.RecommendedDate,
                        IsApproved = result?.IsApproved,
                        ApprovedBy = result?.ApprovedBy,
                        ApprovedDate = result?.ApprovedDate,
                        IsCompleted = result?.IsCompleted,
                        CompletedBy = result?.CompletedBy,
                        CompletedDate = result?.CompletedDate,
                        Tag1 = result?.Tag1,
                        Tag2 = result?.Tag2,
                        Tag3 = result?.Tag3,
                        Tag4 = result?.Tag4,
                        Tag5 = result?.Tag5,

                        IsRewardUpdated = result.IsRewardUpdated,
                        RewardTitle = result.RewardTitle,
                        TotalRewardPoints = result.TotalRewardPoints,
                        RewardMessage = $"You have earned {result.TotalRewardPoints} points for uploading a prescription."
                    };
                
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = fileInfo,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Your prescription (RX) file updated successfully"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Prescription (RX) file details update failed."
                    });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("IsExistsAnyFile")]
        [ProducesDefaultResponseType(typeof(bool))]
        public async Task<IActionResult> IsExistsAnyFile([FromBody] CheckEmptyOrNewUserQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(query, cancellationToken);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "No file upload"
                    });
                }
                return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                {
                    Data = result,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "User has uploaded file"
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("delete-prescription/{id:long}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeletePrescription(long id, [FromBody] DeletePrescriptionCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id != command.PrescriptionId)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid prescription"
                    });
                }
                var result = await _mediator.Send(command, cancellationToken);
                
                return StatusCode(result.StatusCode, new ApiResponseResult
                {
                    Data = result,
                    StatusCode = result.StatusCode,
                    Status = result.Success ? "Success" : "Failed",
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while deleting prescription: {ex.Message}"
                });
            }
        }


        [HttpGet("download/{id:long}")]
        public async Task<IActionResult> DownloadFile(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetPrescriptionDetailsByIdQuery() { Id = id };
                var result = await _mediator.Send(query, cancellationToken);
                if (result is null)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid Id"
                    });
                }
                string filePath = result.FilePath;
                string fileName = Path.GetFileName(filePath);
                var fileRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);
                var prefixCode = Path.GetFileName(fileName)?.Substring(0, 10);
                if (string.IsNullOrEmpty(prefixCode))
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid file name"
                    });
                }

                if (!System.IO.File.Exists(fileRootPath))
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "File not found"
                    });
                }
                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                var matchingFiles = Directory.GetFiles(rootPath)
                                    .Where(f => Path.GetFileName(f).StartsWith(prefixCode))
                                    .ToList();
                if (!matchingFiles.Any())
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "No files found with the given code"
                    });
                }

                // Save reward transaction for download and expose basic info via headers
                var rewardResult = await _mediator.Send(new SaveDownloadRewardTransactionCommand
                {
                    PrescriptionId = id
                }, cancellationToken);

                if (rewardResult is not null && rewardResult.IsRewardUpdated)
                {
                    Response.Headers["X-Reward-Updated"] = "true";
                    if (!string.IsNullOrWhiteSpace(rewardResult.RewardTitle))
                    {
                        Response.Headers["X-Reward-Title"] = rewardResult.RewardTitle;
                    }
                    Response.Headers["X-Reward-Points"] = rewardResult.Points.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    if (!string.IsNullOrWhiteSpace(rewardResult.RewardMessage))
                    {
                        Response.Headers["X-Reward-Message"] = rewardResult.RewardMessage;
                    }
                }

                var memoryStream = new MemoryStream();

                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    foreach (var fPath in matchingFiles)
                    {
                        var entry = zip.CreateEntry(Path.GetFileName(fPath));

                        using var entryStream = entry.Open();
                        using var fileStream = System.IO.File.OpenRead(fPath);
                        fileStream.CopyTo(entryStream);
                    }
                }
                memoryStream.Position = 0;
                var zipFileName = $"{prefixCode}_files.zip";
                return File(memoryStream, "application/zip", zipFileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
