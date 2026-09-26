import { deleteTicket } from '../api';

function TicketRow({ ticket, onDeleted }) {
  const role = localStorage.getItem('role');
  const canDelete = role && role !== 'Customer';

  const handleDelete = async () => {
    if (!window.confirm(`Delete ticket "${ticket.title}"?`)) return;
    try {
      await deleteTicket(ticket.id);
      onDeleted();
    } catch {
      alert('You are not allowed to delete this ticket, or it could not be deleted.');
    }
  };

  return (
    <tr>
      <td>{ticket.title}</td>
      <td>{ticket.status}</td>
      <td>{ticket.categories.join(', ') || '—'}</td>
      <td>
        {canDelete && <button onClick={handleDelete}>Delete</button>}
      </td>
    </tr>
  );
}

export default TicketRow;