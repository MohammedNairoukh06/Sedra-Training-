const BASE_URL = 'https://localhost:56008/api/v1';

export async function getTickets() {
  const res = await fetch(`${BASE_URL}/tickets`);
  return res.json();
}

export async function getTicketById(id) {
  const res = await fetch(`${BASE_URL}/tickets/${id}`);
  return res.json();
}

export async function createTicket(ticket) {
  const res = await fetch(`${BASE_URL}/tickets`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(ticket)
  });
  return res.json();
}