import React, { useState, useEffect } from 'react';
import { getTickets } from '../api/ticketApi';
import TicketFilters from '../components/TicketFilters';
import TicketComments from '../components/TicketComments';

const initialFilters = {
  search: '',
  status: '',
  sortBy: 'createdat_desc',
  page: 1,
  pageSize: 5,
};

export default function TicketsPage() {
  const [filters, setFilters] = useState(initialFilters);
  const [data, setData] = useState({ items: [], totalCount: 0, totalPages: 1, page: 1, pageSize: 5 });
  const [selectedTicketId, setSelectedTicketId] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchTickets = async () => {
    try {
      setLoading(true);
      setError(null);
      const res = await getTickets(filters);
      setData(res);
    } catch (err) {
      setError(err.response?.data?.title || 'Failed to fetch tickets');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTickets();
  }, [filters]);

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= data.totalPages) {
      setFilters((prev) => ({ ...prev, page: newPage }));
    }
  };

  return (
    <div style={{ maxWidth: '960px', margin: '0 auto', padding: '1.5rem' }}>
      <h2>Ticket Management</h2>

      <TicketFilters
        filters={filters}
        onChange={setFilters}
        onReset={() => setFilters(initialFilters)}
      />

      {error && <div style={{ color: '#e53e3e', marginBottom: '1rem' }}>{error}</div>}

      {loading ? (
        <p>Loading tickets...</p>
      ) : (
        <>
          <table style={{ width: '100%', borderCollapse: 'collapse', marginBottom: '1rem' }}>
            <thead>
              <tr style={{ borderBottom: '2px solid #e2e8f0', textAlign: 'left' }}>
                <th style={{ padding: '0.75rem' }}>ID</th>
                <th style={{ padding: '0.75rem' }}>Title</th>
                <th style={{ padding: '0.75rem' }}>Status</th>
                <th style={{ padding: '0.75rem' }}>Created</th>
                <th style={{ padding: '0.75rem' }}>Action</th>
              </tr>
            </thead>
            <tbody>
              {data.items.length === 0 ? (
                <tr>
                  <td colSpan={5} style={{ padding: '1rem', textAlign: 'center', color: '#718096' }}>
                    No tickets found matching current criteria.
                  </td>
                </tr>
              ) : (
                data.items.map((ticket) => (
                  <tr key={ticket.id} style={{ borderBottom: '1px solid #edf2f7' }}>
                    <td style={{ padding: '0.75rem' }}>#{ticket.id}</td>
                    <td style={{ padding: '0.75rem', fontWeight: 500 }}>{ticket.title}</td>
                    <td style={{ padding: '0.75rem' }}>
                      <span
                        style={{
                          padding: '0.25rem 0.5rem',
                          borderRadius: '4px',
                          fontSize: '0.85rem',
                          backgroundColor: ticket.status === 'Resolved' ? '#c6f6d5' : '#e2e8f0',
                        }}
                      >
                        {ticket.status}
                      </span>
                    </td>
                    <td style={{ padding: '0.75rem', fontSize: '0.85rem', color: '#718096' }}>
                      {new Date(ticket.createdAt).toLocaleDateString()}
                    </td>
                    <td style={{ padding: '0.75rem' }}>
                      <button
                        onClick={() => setSelectedTicketId(selectedTicketId === ticket.id ? null : ticket.id)}
                        style={{
                          padding: '0.25rem 0.75rem',
                          backgroundColor: selectedTicketId === ticket.id ? '#4a5568' : '#3182ce',
                          color: '#fff',
                          border: 'none',
                          borderRadius: '4px',
                          cursor: 'pointer',
                        }}
                      >
                        {selectedTicketId === ticket.id ? 'Hide Details' : 'View / Comment'}
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>

          {/* Pagination Controls */}
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <span style={{ fontSize: '0.9rem', color: '#718096' }}>
              Showing Page {data.page} of {data.totalPages} ({data.totalCount} total tickets)
            </span>
            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <button
                disabled={data.page <= 1}
                onClick={() => handlePageChange(data.page - 1)}
                style={{ padding: '0.4rem 0.8rem', cursor: data.page <= 1 ? 'not-allowed' : 'pointer' }}
              >
                Previous
              </button>
              <button
                disabled={data.page >= data.totalPages}
                onClick={() => handlePageChange(data.page + 1)}
                style={{ padding: '0.4rem 0.8rem', cursor: data.page >= data.totalPages ? 'not-allowed' : 'pointer' }}
              >
                Next
              </button>
            </div>
          </div>
        </>
      )}

      {/* Selected Ticket Comment Drawer */}
      {selectedTicketId && (
        <TicketComments ticketId={selectedTicketId} />
      )}
    </div>
  );
}