module.exports = {
    // other config settings...
    module: {
      rules: [
        {
          test: /\.js$/,
          enforce: "pre",
          use: ["source-map-loader"],
          exclude: /node_modules/,   // 👈 Exclude node_modules here
        },
      ],
    },
  };
  