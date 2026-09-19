import { useState } from 'react';
import { createTicket } from '../api';

function TicketForm({ onCreated }) {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    await createTicket({ title, description, userId: 1, categoryIds: [] });
    setTitle('');
    setDescription('');
    onCreated();
  };

  return (
    <form onSubmit={handleSubmit}>
      <input value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Title" required />
      <input value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Description" required />
      <button type="submit">Create Ticket</button>
    </form>
  );
}

export default TicketForm;