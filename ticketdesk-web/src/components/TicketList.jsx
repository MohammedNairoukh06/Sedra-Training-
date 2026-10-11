import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getTickets } from '../api';
import TicketList from '../components/TicketList';
import TicketForm from '../components/TicketForm';
import LogoutButton from '../components/LogoutButton';

function TicketListPage() {
  const [tickets, setTickets] = useState([]);
  const [pagination, setPagination] = useState({ page: 1, pageSize: 5, totalPages: 1, totalCount: 0 });
  const [search, setSearch] = useState('');
  const [status, setStatus] = useState('');
  const [sortBy, setSortBy] = useState('createdAt_desc');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const role = localStorage.getItem('role');
  const fullName = localStorage.getItem('fullName');

  const fetchTickets = (pageNumber = 1) => {
    setLoading(true);
    setError(null);

    const params = {
      page: pageNumber,
      pageSize: 5,
      sortBy,
    };

    if (search.trim()) params.search = search.trim();
    if (status) params.status = status;

    getTickets(params)
      .then((data) => {
        if (data && Array.isArray(data.items)) {
          setTickets(data.items);
          setPagination({
            page: data.page,
            pageSize: data.pageSize,
            totalPages: data.totalPages,
            totalCount: data.totalCount,
          });
        } else if (Array.isArray(data)) {
          setTickets(data);
        } else {
          setTickets([]);
        }
        setLoading(false);
      })
      .catch((err) => {
        setError(err.response?.data?.innerError || err.response?.data?.title || 'Failed to load tickets.');
        setTickets([]);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchTickets(1);
  }, [status, sortBy]);

  const handleSearchSubmit = (e) => {
    e.preventDefault();
    fetchTickets(1);
  };

  return (
    <div style={{ maxWidth: '900px', margin: '0 auto', padding: '1rem', textAlign: 'left' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
        <p style={{ margin: 0 }}>
          Logged in as <strong>{fullName || 'user'}</strong> ({role})
        </p>
        <LogoutButton />
      </div>

      <h1>Tickets</h1>

      <TicketForm onCreated={() => fetchTickets(pagination.page)} />

      <form onSubmit={handleSearchSubmit} style={{ display: 'flex', gap: '0.75rem', margin: '1.5rem 0', flexWrap: 'wrap' }}>
        <input
          type="text"
          placeholder="Search title or description..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          style={{ padding: '0.5rem', flex: 1, minWidth: '220px', borderRadius: '4px', border: '1px solid #ccc' }}
        />

        <select
          value={status}
          onChange={(e) => setStatus(e.target.value)}
          style={{ padding: '0.5rem', borderRadius: '4px', border: '1px solid #ccc' }}
        >
          <option value="">All Statuses</option>
          <option value="Open">Open</option>
          <option value="InProgress">In Progress</option>
          <option value="Resolved">Resolved</option>
          <option value="Closed">Closed</option>
        </select>

        <select
          value={sortBy}
          onChange={(e) => setSortBy(e.target.value)}
          style={{ padding: '0.5rem', borderRadius: '4px', border: '1px solid #ccc' }}
        >
          <option value="createdAt_desc">Newest First</option>
          <option value="createdAt_asc">Oldest First</option>
          <option value="title_asc">Title (A-Z)</option>
          <option value="title_desc">Title (Z-A)</option>
          <option value="status_asc">Status (A-Z)</option>
        </select>

        <button
          type="submit"
          style={{ padding: '0.5rem 1rem', backgroundColor: '#3182ce', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
        >
          Search
        </button>
      </form>

      {loading && <p>Loading…</p>}
      {error && <p style={{ color: '#e53e3e' }}>{error}</p>}

      {!loading && !error && (
        <>
          <TicketList tickets={tickets} onDeleted={() => fetchTickets(pagination.page)} />

          <div style={{ marginTop: '1rem', display: 'flex', flexDirection: 'column', gap: '0.25rem' }}>
            {tickets.map((t) => (
              <div key={t.id}>
                <Link to={`/tickets/${t.id}`}>View Details #{t.id}</Link>
              </div>
            ))}
          </div>

          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '1.5rem', paddingTop: '1rem', borderTop: '1px solid #eee' }}>
            <span style={{ fontSize: '0.9rem', color: '#666' }}>
              Page {pagination.page} of {pagination.totalPages} ({pagination.totalCount} total tickets)
            </span>
            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <button
                type="button"
                disabled={pagination.page <= 1}
                onClick={() => fetchTickets(pagination.page - 1)}
                style={{ padding: '0.4rem 0.8rem', cursor: pagination.page <= 1 ? 'not-allowed' : 'pointer' }}
              >
                Previous
              </button>
              <button
                type="button"
                disabled={pagination.page >= pagination.totalPages}
                onClick={() => fetchTickets(pagination.page + 1)}
                style={{ padding: '0.4rem 0.8rem', cursor: pagination.page >= pagination.totalPages ? 'not-allowed' : 'pointer' }}
              >
                Next
              </button>
            </div>
          </div>
        </>
      )}
    </div>
  );
}

export default TicketListPage;