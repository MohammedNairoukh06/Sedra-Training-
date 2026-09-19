function TicketRow({ ticket }) {
  return (
    <tr>
      <td>{ticket.title}</td>
      <td>{ticket.status}</td>
      <td>{ticket.categories.join(', ') || '—'}</td>
    </tr>
  );
}

export default TicketRow;