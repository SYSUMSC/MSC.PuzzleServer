const createProxyMiddleware = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://puzzle.sysums.club';

const httpEndpoints = ['/api', '/swagger'];
const wsEndpoints = ['/hub'];

module.exports = function (app) {
  const httpProxy = createProxyMiddleware(httpEndpoints, {
    target: target,
    secure: false,
    changeOrigin: true
  });

  const wsProxy = createProxyMiddleware(wsEndpoints, {
    target: target,
    secure: false,
    changeOrigin: true,
    ws: true
  });

  app.use(httpProxy);
  app.use(wsProxy);
};
