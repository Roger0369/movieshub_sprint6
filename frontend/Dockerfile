FROM node:22.14.0 AS builder
WORKDIR /app
COPY . .
RUN npm install && npm run build 

# Etapa de producción (Nginx para servir Angular)
FROM nginx:latest
COPY --from=builder /app/dist/login-frontend /usr/share/nginx/html
EXPOSE 3000
CMD ["nginx", "-g", "daemon off;"]