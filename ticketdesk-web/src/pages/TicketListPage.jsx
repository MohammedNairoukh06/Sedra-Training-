import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getTickets } from '../api';
import TicketList from '../components/TicketList';
import TicketForm from '../components/TicketForm';

function TicketListPage() {
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchTickets = () => {
    setLoading(true);
    getTickets()
      .then((data) => {
        setTickets(data);
        setLoading(false);
      })
      .catch(() => {
        setError('Failed to load tickets.');
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchTickets();
  }, []);

  return (
    <div>
      <h1>Tickets</h1>
      <TicketForm onCreated={fetchTickets} />
      {loading && <p>Loading…</p>}
      {error && <p>{error}</p>}
      {!loading && !error && (
        <>
          <TicketList tickets={tickets} onDeleted={fetchTickets} />
          {tickets.map((t) => (
            <div key={t.id}>
              <Link to={`/tickets/${t.id}`}>View #{t.id}</Link>
            </div>
          ))}
        </>
      )}
    </div>
  );
}

export default TicketListPage;