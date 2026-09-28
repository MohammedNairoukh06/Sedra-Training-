const BASE_URL = 'https://localhost:56008/api';

function authHeaders() {
  const token = localStorage.getItem('token');
  return token ? { Authorization: `Bearer ${token}` } : {};
}

// Shared helper for the Week 7 endpoints: handles 401, and turns server errors into Error(message)
async function request(path, options = {}) {
  const res = await fetch(`${BASE_URL}${path}`, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...authHeaders(),
      ...(options.headers || {})
    }
  });

  if (res.status === 401) {
    logout();
    window.location.href = '/login';
    throw new Error('Session expired. Please log in again.');
  }

  if (!res.ok) {
    let message = null;
    try {
      const problem = await res.json();
      message = problem.title || problem.message || null;
    } catch {
      // empty body (for example a plain 403 or 404)
    }

    if (!message) {
      message =
        res.status === 403
          ? 'You are not allowed to do that.'
          : `Request failed (${res.status}) for ${path}.`;
    }

    throw new Error(message);
  }

  if (res.status === 204) return null;
  return res.json();
}

// ---------- Auth ----------

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
  localStorage.setItem('fullName', data.fullName);
  return data;
}

export function logout() {
  localStorage.removeItem('token');
  localStorage.removeItem('role');
  localStorage.removeItem('fullName');
}

// ---------- Tickets (Week 5/6) ----------

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
  if (res.status === 401) {
    logout();
    window.location.href = '/login';
    return null;
  }
  if (!res.ok) return null;
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

// ---------- Team & assignment (Week 7) ----------

export const getAssignments = (id) => request(`/v1/tickets/${id}/assignments`);

export const getAgents = () => request('/users/agents');

export const getMyManagers = () => request('/users/my-managers');

export const assignAgents = (id, userIds) =>
  request(`/v1/tickets/${id}/agents`, { method: 'POST', body: JSON.stringify({ userIds }) });

export const assignManagers = (id, userIds) =>
  request(`/v1/tickets/${id}/managers`, { method: 'POST', body: JSON.stringify({ userIds }) });

// ---------- Status workflow (Week 7) ----------

export const changeStatus = (id, status) =>
  request(`/v1/tickets/${id}/status`, { method: 'POST', body: JSON.stringify({ status }) });

export const closeTicket = (id) => request(`/v1/tickets/${id}/close`, { method: 'POST' });

export const returnTicket = (id) => request(`/v1/tickets/${id}/return`, { method: 'POST' });