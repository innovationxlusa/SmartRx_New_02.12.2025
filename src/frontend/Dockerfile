FROM node:20.19.1-slim
WORKDIR /tmp
RUN corepack enable && corepack prepare yarn@4.9.1 --activate
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 4200
CMD ["yarn","start","--host", "0.0.0.0", "--disable-host-check"]