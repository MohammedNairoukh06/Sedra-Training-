/* =========================================================
   TicketDesk - report_queries.sql
   Week 2, Day 5 - Report queries
   ========================================================= */

-- 1) Tickets per status
SELECT Status, COUNT(*) AS TicketCount
FROM Ticket
GROUP BY Status
ORDER BY TicketCount DESC;

-- 2) Tickets per user (customer who opened them)
SELECT u.Username, COUNT(t.TicketId) AS TicketCount
FROM [User] u
LEFT JOIN Ticket t ON t.UserId = u.UserId
GROUP BY u.Username
ORDER BY TicketCount DESC;

-- 3) Tickets per category
SELECT c.CategoryName, COUNT(tc.TicketId) AS TicketCount
FROM Category c
LEFT JOIN TicketCategory tc ON tc.CategoryId = c.CategoryId
GROUP BY c.CategoryName
ORDER BY TicketCount DESC;
