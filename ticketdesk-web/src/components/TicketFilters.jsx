import React from 'react';

export default function TicketFilters({ filters, onChange, onReset }) {
  const handleChange = (field, value) => {
    onChange({ ...filters, [field]: value, page: 1 });
  };

  return (
    <div
      style={{
        display: 'flex',
        flexWrap: 'wrap',
        gap: '0.75rem',
        padding: '1rem',
        backgroundColor: '#f8fafc',
        borderRadius: '8px',
        marginBottom: '1.5rem',
        alignItems: 'center',
      }}
    >
      <input
        type="text"
        placeholder="Search title or description..."
        value={filters.search || ''}
        onChange={(e) => handleChange('search', e.target.value)}
        style={{ padding: '0.5rem', borderRadius: '4px', border: '1px solid #cbd5e0', minWidth: '220px' }}
      />

      <select
        value={filters.status || ''}
        onChange={(e) => handleChange('status', e.target.value)}
        style={{ padding: '0.5rem', borderRadius: '4px', border: '1px solid #cbd5e0' }}
      >
        <option value="">All Statuses</option>
        <option value="Open">Open</option>
        <option value="InProgress">In Progress</option>
        <option value="Resolved">Resolved</option>
        <option value="Closed">Closed</option>
      </select>

      <select
        value={filters.sortBy || 'createdat_desc'}
        onChange={(e) => handleChange('sortBy', e.target.value)}
        style={{ padding: '0.5rem', borderRadius: '4px', border: '1px solid #cbd5e0' }}
      >
        <option value="createdat_desc">Newest First</option>
        <option value="createdat_asc">Oldest First</option>
        <option value="title_asc">Title (A-Z)</option>
        <option value="title_desc">Title (Z-A)</option>
        <option value="status_asc">Status (A-Z)</option>
      </select>

      <button
        onClick={onReset}
        style={{
          padding: '0.5rem 1rem',
          backgroundColor: '#e2e8f0',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer',
        }}
      >
        Reset Filters
      </button>
    </div>
  );
}