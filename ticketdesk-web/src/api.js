const BASE_URL = 'https://localhost:56008/api';

function authHeaders() {
  const token = localStorage.getItem('token');
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export async function login(email, password) {
  const res = await fetch(`${BASE_URL}/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  if (!res.ok) throw new Error('Invalid credentials');
  const data = await res.json();
  localStorage.setItem('token', data.token);
  localStorage.setItem('role', data.role);
  return data;
}

export function logout() {
  localStorage.removeItem('token');
  localStorage.removeItem('role');
}

export async function getTickets() {
  const res = await fetch(`${BASE_URL}/v1/tickets`, { headers: authHeaders() });
  if (res.status === 401) {
    logout();
    window.location.href = '/login';
    return [];
  }
  return res.json();
}

export async function getTicketById(id) {
  const res = await fetch(`${BASE_URL}/v1/tickets/${id}`, { headers: authHeaders() });
  return res.json();
}

export async function createTicket(ticket) {
  const res = await fetch(`${BASE_URL}/v1/tickets`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', ...authHeaders() },
    body: JSON.stringify(ticket)
  });
  if (!res.ok) {
    const problem = await res.json();
    throw problem;
  }
  return res.json();
}

export async function deleteTicket(id) {
  const res = await fetch(`${BASE_URL}/v1/tickets/${id}`, {
    method: 'DELETE',
    headers: authHeaders()
  });
  if (!res.ok && res.status !== 204) {
    throw new Error('Delete failed');
  }
}