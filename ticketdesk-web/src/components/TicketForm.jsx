import { useState } from 'react';
import { createTicket } from '../api';

function TicketForm({ onCreated }) {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [errors, setErrors] = useState({});

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors({});

    const res = await fetch('https://localhost:56008/api/v1/tickets', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title, description, userId: 1, categoryIds: [] })
    });

    if (!res.ok) {
      const problem = await res.json();
      setErrors(problem.errors || {});
      return;
    }

    setTitle('');
    setDescription('');
    onCreated();
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <input value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Title" />
        {errors.Title && <p style={{ color: 'red' }}>{errors.Title[0]}</p>}
      </div>
      <div>
        <input value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Description" />
        {errors.Description && <p style={{ color: 'red' }}>{errors.Description[0]}</p>}
      </div>
      <button type="submit">Create Ticket</button>
    </form>
  );
}

export default TicketForm;