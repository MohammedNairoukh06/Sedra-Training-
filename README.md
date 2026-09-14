* Configure an AutoMapper profile to map between Domain entities and DTOs.
* **Deliverable:** The Service layer executes business operations and returns mapped DTOs.

### Day 4: Full CRUD Endpoints (Thin Controller)
* **Focus:** RESTful actions, HTTP status codes, and asynchronous actions.
* **Tasks:**
  * Build `TicketsController` with the 5 CRUD endpoints (`GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`).
  * Implement appropriate response types (`Ok`, `NotFound`, `CreatedAtAction`, `NoContent`).
  * Verify end-to-end data creation and persistence via Swagger.
* **Deliverable:** End-to-end CRUD operations work across API $\rightarrow$ Service $\rightarrow$ Repository layers.

### Day 5: API Versioning + Final Verification
* **Focus:** API lifecycle management and production readiness.
* **Tasks:**
  * Install `Asp.Versioning.Mvc` and configure URL-segment versioning (`/api/v1/tickets`).
  * Verify all versioned endpoints via Swagger.
  * Conduct a code cleanup, ensure clean Git history, and submit the final Pull Request.
* **Deliverable:** A fully tested, versioned Clean Architecture Web API merged via PR.
