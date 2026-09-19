import { useState, useEffect } from 'react';
import Greeting from './components/Greeting';
import Counter from './components/Counter';
import TicketList from './components/TicketList';
import TicketForm from './components/TicketForm';

function App() {
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchTickets = () => {
    setLoading(true);
    fetch('https://localhost:56008/api/v1/tickets')
      .then((res) => res.json())
      .then((data) => {
        setTickets(data);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchTickets();
  }, []);

  return (
    <div>
      <Greeting name="Mohammed" />
      <Counter />
      <h1>Tickets</h1>
      <TicketForm onCreated={fetchTickets} />
      {loading ? <p>Loading…</p> : <TicketList tickets={tickets} />}
    </div>
  );
}

export default App;