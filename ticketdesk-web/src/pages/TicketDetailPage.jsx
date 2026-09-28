import { useState, useEffect, useCallback } from 'react';
import { useParams, Link } from 'react-router-dom';
import LogoutButton from '../components/LogoutButton';
import {
  getTicketById,
  getAssignments,
  getAgents,
  getMyManagers,
  assignAgents,
  assignManagers,
  changeStatus,
  closeTicket,
  returnTicket
} from '../api';

function TicketDetailPage() {
  const { id } = useParams();
  const role = localStorage.getItem('role');
  const fullName = localStorage.getItem('fullName');

  const [ticket, setTicket] = useState(null);
  const [assignments, setAssignments] = useState({ agents: [], managers: [] });
  const [options, setOptions] = useState([]);
  const [selectedIds, setSelectedIds] = useState([]);
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  const load = useCallback(async () => {
    const t = await getTicketById(id);
    setTicket(t);
    if (!t) {
      setLoading(false);
      return;
    }

    const a = await getAssignments(id);
    setAssignments(a);

    if (role === 'Admin') {
      const agents = await getAgents();
      setOptions(agents);
      setSelectedIds(a.agents.map((x) => x.id));
    } else if (role === 'Agent') {
      const managers = await getMyManagers();
      setOptions(managers);
      // only pre-tick managers that belong to this agent
      setSelectedIds(
        a.managers.map((x) => x.id).filter((mid) => managers.some((m) => m.id === mid))
      );
    }

    setLoading(false);
  }, [id, role]);

  useEffect(() => {
    load().catch((e) => {
      setError(e.message);
      setLoading(false);
    });
  }, [load]);

  const toggleSelected = (userId) => {
    setSelectedIds((prev) =>
      prev.includes(userId) ? prev.filter((x) => x !== userId) : [...prev, userId]
    );
  };

  // Runs an API action, shows the result, then refreshes the ticket
  const run = async (action, successText) => {
    setError('');
    setMessage('');
    try {
      await action();
      setMessage(successText);
      await load();
    } catch (e) {
      setError(e.message);
    }
  };

  if (loading) return <p>Loading…</p>;
  if (!ticket) return <p>Ticket not found. <Link to="/">← Back to list</Link></p>;

  const isAgentOrAdmin = role === 'Agent' || role === 'Admin';
  const canWork = role === 'Manager' || isAgentOrAdmin;

  return (
    <div>
      <p>
        Logged in as {fullName || 'user'} ({role}) <LogoutButton />
      </p>
      <Link to="/">← Back to list</Link>

      <h1>{ticket.title}</h1>
      <p>{ticket.description}</p>
      <p><strong>Status: {ticket.status}</strong></p>
      <p>Categories: {ticket.categories.join(', ') || '—'}</p>

      {message && <p style={{ color: 'lightgreen' }}>{message}</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      <h2>Assigned team</h2>
      <p>Agents: {assignments.agents.map((a) => a.fullName).join(', ') || 'none'}</p>
      <p>Managers: {assignments.managers.map((m) => m.fullName).join(', ') || 'none'}</p>

      {/* Admin: assign agents */}
      {role === 'Admin' && (
        <div>
          <h3>Assign agents</h3>
          {options.length === 0 && <p>No agents exist yet.</p>}
          {options.map((agent) => (
            <label key={agent.id} style={{ display: 'block' }}>
              <input
                type="checkbox"
                checked={selectedIds.includes(agent.id)}
                onChange={() => toggleSelected(agent.id)}
              />{' '}
              {agent.fullName}
            </label>
          ))}
          <button
            onClick={() => run(() => assignAgents(id, selectedIds), 'Agents updated.')}
          >
            Save agents
          </button>
        </div>
      )}

      {/* Agent: share with own managers */}
      {role === 'Agent' && (
        <div>
          <h3>Share with my managers</h3>
          {options.length === 0 && <p>You have no managers yet.</p>}
          {options.map((manager) => (
            <label key={manager.id} style={{ display: 'block' }}>
              <input
                type="checkbox"
                checked={selectedIds.includes(manager.id)}
                onChange={() => toggleSelected(manager.id)}
              />{' '}
              {manager.fullName}
            </label>
          ))}
          <button
            onClick={() => run(() => assignManagers(id, selectedIds), 'Managers updated.')}
          >
            Save managers
          </button>
        </div>
      )}

      {/* Workflow buttons: each one only for the role allowed to use it */}
      <h3>Workflow</h3>

      {canWork && ticket.status === 'Open' && (
        <button onClick={() => run(() => changeStatus(id, 'InProgress'), 'Ticket is now In Progress.')}>
          Start work
        </button>
      )}

      {canWork && ticket.status === 'InProgress' && (
        <button onClick={() => run(() => changeStatus(id, 'Resolved'), 'Ticket marked Resolved.')}>
          Mark resolved
        </button>
      )}

      {isAgentOrAdmin && ticket.status === 'Resolved' && (
        <>
          <button onClick={() => run(() => closeTicket(id), 'Ticket closed.')}>Close ticket</button>{' '}
          <button onClick={() => run(() => returnTicket(id), 'Ticket returned to the manager.')}>
            Return to manager
          </button>
        </>
      )}

      {role === 'Manager' && ticket.status === 'Resolved' && (
        <p>Waiting for the agent to close or return this ticket.</p>
      )}

      {ticket.status === 'Closed' && <p>This ticket is closed.</p>}
    </div>
  );
}

export default TicketDetailPage;