import React, { useState, useEffect } from 'react';
import { getTicketComments, addTicketComment } from '../api/ticketApi';

export default function TicketComments({ ticketId }) {
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  const loadComments = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await getTicketComments(ticketId);
      setComments(data);
    } catch (err) {
      setError(err.response?.data?.title || 'Failed to load comments');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (ticketId) {
      loadComments();
    }
  }, [ticketId]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!newComment.trim()) return;

    try {
      setSubmitting(true);
      await addTicketComment(ticketId, newComment.trim());
      setNewComment('');
      await loadComments();
    } catch (err) {
      setError(err.response?.data?.title || 'Failed to post comment');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div style={{ marginTop: '2rem', padding: '1rem', border: '1px solid #e2e8f0', borderRadius: '8px' }}>
      <h3 style={{ marginBottom: '1rem' }}>Ticket Discussion</h3>

      {error && (
        <div style={{ color: '#e53e3e', marginBottom: '1rem' }}>
          {error}
        </div>
      )}

      {loading ? (
        <p>Loading comments...</p>
      ) : comments.length === 0 ? (
        <p style={{ color: '#718096' }}>No comments on this ticket yet.</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem', marginBottom: '1.5rem' }}>
          {comments.map((comment) => (
            <div
              key={comment.id}
              style={{
                backgroundColor: '#f7fafc',
                padding: '0.75rem 1rem',
                borderRadius: '6px',
                borderLeft: '4px solid #3182ce',
              }}
            >
              <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.85rem', color: '#4a5568' }}>
                <strong>{comment.authorName || `User #${comment.userId}`}</strong>
                <span>{new Date(comment.createdAt).toLocaleString()}</span>
              </div>
              <p style={{ marginTop: '0.5rem', margin: 0, color: '#2d3748' }}>{comment.content}</p>
            </div>
          ))}
        </div>
      )}

      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
        <textarea
          rows={3}
          value={newComment}
          onChange={(e) => setNewComment(e.target.value)}
          placeholder="Write a comment..."
          style={{
            width: '100%',
            padding: '0.5rem',
            borderRadius: '4px',
            border: '1px solid #cbd5e0',
            fontFamily: 'inherit',
          }}
          disabled={submitting}
        />
        <button
          type="submit"
          disabled={submitting || !newComment.trim()}
          style={{
            alignSelf: 'flex-end',
            padding: '0.5rem 1.25rem',
            backgroundColor: '#3182ce',
            color: '#fff',
            border: 'none',
            borderRadius: '4px',
            cursor: submitting ? 'not-allowed' : 'pointer',
          }}
        >
          {submitting ? 'Posting...' : 'Post Comment'}
        </button>
      </form>
    </div>
  );
}