import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './index.css';
import TicketListPage from './pages/TicketListPage';
import TicketDetailPage from './pages/TicketDetailPage';
import LoginPage from './pages/LoginPage';
import RequireAuth from './components/RequireAuth';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/" element={<RequireAuth><TicketListPage /></RequireAuth>} />
        <Route path="/tickets/:id" element={<RequireAuth><TicketDetailPage /></RequireAuth>} />
      </Routes>
    </BrowserRouter>
  </StrictMode>
);