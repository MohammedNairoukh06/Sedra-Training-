/* =========================================================
   TicketDesk - Day 3 practice
   Keys & Relationships - JOIN queries
   ========================================================= */

-- 1) List each ticket together with the name of the user who opened it
SELECT
    t.TicketId,
    t.Title,
    t.Status,
    u.Username AS OpenedBy
FROM Ticket t
INNER JOIN [User] u ON t.UserId = u.UserId
ORDER BY t.TicketId;

-- 2) Number of tickets per user (JOIN + GROUP BY)
SELECT
    u.Username,
    COUNT(t.TicketId) AS NumberOfTickets
FROM [User] u
LEFT JOIN Ticket t ON t.UserId = u.UserId
GROUP BY u.Username
ORDER BY NumberOfTickets DESC;
