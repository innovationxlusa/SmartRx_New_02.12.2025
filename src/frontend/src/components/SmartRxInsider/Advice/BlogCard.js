import React from "react";

const BlogCard = ({ post }) => {
    return (
        <div className="blog-card">
            <div className="blog-card-img-wrapper">
                <img
                    src={post.imageUrl}
                    alt={post.title}
                    onError={(e) => {
                        e.currentTarget.style.display = "none";
                        e.currentTarget.parentNode.style.backgroundImage =
                            "url('https://specialcarepr.com/wp-content/uploads/2017/04/e-Prescribing-para-mayor-eficiencia.jpg')";
                        e.currentTarget.parentNode.style.backgroundSize =
                            "cover";
                        e.currentTarget.parentNode.style.backgroundPosition =
                            "center";
                    }}
                />
            </div>
            <div className="blog-card-content">
                <h3>{post.title}</h3>
                <p className="meta">
                    By <strong>{post.author}</strong> •{" "}
                    {new Date(post.date).toLocaleDateString()}
                </p>
                <p className="excerpt">{post.excerpt}</p>
                <button className="read-more">Read More →</button>
            </div>
        </div>
    );
};

export default BlogCard;
