const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'http://localhost:7000',
      changeOrigin: true,
      secure: false, // Disable SSL verification for development
      logLevel: 'debug',
      onError: function (err, req, res) {
        console.error('Proxy error:', err);
      },
      onProxyReq: function (proxyReq, req, res) {
        console.log('Proxying request to:', proxyReq.getHeader('host') + proxyReq.path);
      }
    })
  );
};
