import TicketRow from './TicketRow';

function TicketList({ tickets, onDeleted }) {
  return (
    <table border="1" cellPadding="8">
      <thead>
        <tr>
          <th>Title</th>
          <th>Status</th>
          <th>Categories</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {tickets.map(ticket => (
          <TicketRow key={ticket.id} ticket={ticket} onDeleted={onDeleted} />
        ))}
      </tbody>
    </table>
  );
}

export default TicketList;