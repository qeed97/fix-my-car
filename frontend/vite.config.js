import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from "@tailwindcss/vite";

// https://vite.dev/config/
export default defineConfig({
    plugins: [
      tailwindcss(),
      react()],
    server: {
        watch: {
            usePolling: true,
        },
        host: true,
        strictPort: true,
        port: 5173,
        proxy: {
            '/api': {
                target: 'http://backend-server:8080/',
                changeOrigin: true,
                rewrite: path => path.replace(/^\/api/, '')
            }
        }
    }
})
