import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { AuthProvider } from 'react-oidc-context'
import App from './App.tsx'
import './index.css'

const oidcConfig = {
    authority: 'http://localhost:5001/', // from config
    client_id: 'spa',
    redirect_uri: 'http://localhost:5173/auth/callback',
    response_type: 'code',
    scope: 'openid profile email api', // offline_access
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
        <App />
  </StrictMode>,
)
