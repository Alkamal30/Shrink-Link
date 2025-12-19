import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Container, createTheme, ThemeProvider } from '@mui/material';
import Home from './components/Home';
import Register from './components/Register';
import Login from './components/Login';
import Header from './components/Header';
import './App.css'
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import AuthCallback from './components/AuthCallback';
function App() {
    const theme = createTheme({

    });

    return (
        <ThemeProvider theme={theme}>
            <BrowserRouter>
                <Header />
                <Container sx={{
                    width: '100%',
                    height: '100%',
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center'
                }}>
                    <Routes>
                        <Route path='/' element={<Home />} />
                        <Route path='/login' element={<Login />} />
                        <Route path='/register' element={<Register />} />
                        <Route path='/auth/callback' element={<AuthCallback />} />
                    </Routes>
                </Container>
            </BrowserRouter>
        </ThemeProvider>
    )
}

export default App;
