import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getTicketById } from '../api';

function TicketDetailPage() {
  const { id } = useParams();
  const [ticket, setTicket] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getTicketById(id).then((data) => {
      setTicket(data);
      setLoading(false);
    });
  }, [id]);

  if (loading) return <p>Loading…</p>;
  if (!ticket) return <p>Ticket not found.</p>;

  return (
    <div>
      <Link to="/">← Back to list</Link>
      <h1>{ticket.title}</h1>
      <p>{ticket.description}</p>
      <p>Status: {ticket.status}</p>
      <p>Categories: {ticket.categories.join(', ') || '—'}</p>
    </div>
  );
}

export default TicketDetailPage;