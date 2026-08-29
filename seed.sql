/* =========================================================
   TicketDesk - seed.sql
   Week 2, Day 5 - Seeding starter data
   ========================================================= */

-- Roles
INSERT INTO Role (RoleName) VALUES
('Admin'),
('Agent'),
('Customer');

-- Categories
INSERT INTO Category (CategoryName) VALUES
('Billing'),
('Technical'),
('Account'),
('General');

-- Users
INSERT INTO [User] (Username, Email, PasswordHash) VALUES
('mzeidan',   'm.zeidan@ticketdesk.com',   'hash_1'),
('slama',     's.lama@ticketdesk.com',     'hash_2'),
('aomar',     'a.omar@ticketdesk.com',     'hash_3'),
('rkhaled',   'r.khaled@ticketdesk.com',   'hash_4');

-- User <-> Role assignments
INSERT INTO UserRole (UserId, RoleId) VALUES
(1, 1),  -- mzeidan  -> Admin
(2, 2),  -- slama    -> Agent
(3, 3),  -- aomar    -> Customer
(4, 3);  -- rkhaled  -> Customer

-- Tickets
INSERT INTO Ticket (Title, Description, Status, UserId) VALUES
('Cannot log in to account',        'Password reset link is not arriving.',        'Open',       3),
('Invoice amount looks wrong',      'Charged twice for July subscription.',        'InProgress', 3),
('Feature request: dark mode',      'Would like a dark theme for the dashboard.',  'Open',       4),
('App crashes on ticket submit',    'Crash occurs on the submit button.',          'Resolved',   4),
('Update billing address',          'Need to change the billing address on file.', 'Closed',     3);

-- Ticket <-> Category assignments
INSERT INTO TicketCategory (TicketId, CategoryId) VALUES
(1, 3),  -- login issue        -> Account
(2, 1),  -- invoice issue      -> Billing
(3, 4),  -- feature request    -> General
(4, 2),  -- app crash          -> Technical
(5, 1);  -- billing address    -> Billing

-- Ticket comments
INSERT INTO TicketComment (TicketId, UserId, CommentText) VALUES
(1, 2, 'Checked the mail server, resending the reset link now.'),
(1, 3, 'Still have not received it, please advise.'),
(2, 2, 'Confirmed duplicate charge, refund has been submitted.'),
(4, 2, 'Fixed in the latest build, please update the app.');
