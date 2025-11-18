# Datwise - Security & Safety Control Panel

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-Production%20Ready-brightgreen)

A professional Security & Safety Control Panel application built with ASP.NET Core Razor Pages and a RESTful API. Manage and track security and safety incidents with an intuitive dashboard, real-time statistics, and powerful filtering and sorting capabilities.

## ? Key Features

### Dashboard & UI
- **Real-time Statistics** - Total open issues, critical/high severity counts, monthly resolutions
- **Professional Design** - Modern, responsive UI with smooth animations
- **Dark/Light Mode** - Toggle theme with persistent user preference
- **Full-Screen Optimized** - Perfect fit within viewport, no scrolling needed
- **Column Sorting** - Click any table header to sort ascending/descending
- **Multi-Filter Support** - Comma-separated status/severity filtering (e.g., `status=Open,In Progress`)

### Data Management
- **Issue Tracking** - Create, read, update, and delete security/safety incidents
- **Advanced Filtering** - Status, severity, department, location filters with "contains" logic
- **Smart Statistics** - Automatic calculation of metrics and trends
- **SQLite Database** - Lightweight, file-based, auto-initialized on startup

### Technical Architecture
- **3-Tier Architecture** - Clean separation: UI ? API ? Data
- **RESTful API** - Complete REST endpoints with OpenAPI/Swagger documentation
- **Entity Framework Core** - ORM with migrations and automatic database creation
- **Comprehensive Tests** - 35 unit tests covering repositories and services
- **Async/Await Throughout** - All I/O operations are non-blocking

## ?? Quick Start

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 or VS Code

### Setup (5 minutes)

1. **Clone & Navigate**
```bash
git clone https://github.com/ronanoshi/Datwise.git
cd Datwise
```

2. **Start API** (Terminal 1)
```bash
cd Datwise.Api
dotnet run
# Listens on: http://localhost:53487 (dev) or https://localhost:53486
```

3. **Start WebForms** (Terminal 2)
```bash
cd Datwise.WebForms
dotnet run
# Listens on: http://localhost:53488 (dev) or https://localhost:53485
```

4. **Open Browser**
```
http://localhost:53488
```

## ?? API Endpoints

### Base URL
`http://localhost:53487` (development) or `https://localhost:53486` (production)

### Main Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/issues` | Get all issues with optional filtering & sorting |
| GET | `/api/issues/{id}` | Get issue by ID |
| GET | `/api/issues/statistics/summary` | Get dashboard statistics |
| POST | `/api/issues` | Create new issue |
| PUT | `/api/issues/{id}` | Update issue |
| DELETE | `/api/issues/{id}` | Delete issue |

### Query Parameters

**Status Filter** (comma-separated)
```
GET /api/issues?status=Open,In Progress
GET /api/issues?status=Open,In Progress,Resolved
```

**Severity Filter** (comma-separated)
```
GET /api/issues?severity=High,Critical
GET /api/issues?severity=Low,Medium,High,Critical
```

**Combined with Sorting**
```
GET /api/issues?status=Open,In Progress&severity=High,Critical&sort=-date
GET /api/issues?status=Open&sort=title
```

**Valid Sort Fields**: `id`, `title`, `severity`, `status`, `department`, `location`, `reportedby`, `date`
Use `-` prefix for descending: `sort=-date`

## ??? Architecture

### Project Structure
```
Datwise/
??? Datwise.Api/              # REST API (ASP.NET Core Web API)
??? Datwise.WebForms/         # UI (Razor Pages)
??? Datwise.Services/         # Business logic (.NET Standard 2.0)
??? Datwise.Data/             # Data access (EF Core)
??? Datwise.Contracts/        # Interfaces & DTOs
??? Datwise.Models/           # Domain models
??? Datwise.Tests/            # Unit tests (xUnit + Moq)
```

### Data Flow
```
User Form (Razor Pages)
    ?
ReportIssueModel (HTTP POST)
    ?
IssuesController (REST API)
    ?
IssueService (Business Logic)
    ?
IssueRepository (EF Core)
    ?
SQLite Database
```

## ?? UI/UX Features

### Dark/Light Mode
- Located in navbar toggle switch
- Persisted in browser localStorage
- Smooth CSS variable transitions
- Works across all pages

### Table Sorting
- Click column headers to sort
- Visual indicators (? ?) show direction
- URL persists sort state: `/?sort=-date`
- Supports all issue fields

### Statistics Dashboard
- Real-time metrics cards
- Color-coded severity indicators
- Open issues count (Open + In Progress)
- Critical/High/Medium/Low breakdowns
- Monthly resolution tracking

## ?? Testing

### Run All Tests
```bash
dotnet test Datwise.Tests
```

### Test Coverage
- **35 total tests** - All passing
- **17 repository tests** - CRUD, filtering, sorting
- **18 service tests** - Business logic, validation

### Test Framework
- **xUnit** - Modern test framework
- **Moq** - Mocking library for dependencies
- **In-Memory Database** - Isolated test data

## ?? Sample Data

Application comes pre-seeded with 5 issues:

| Issue | Severity | Status | Department |
|-------|----------|--------|-----------|
| Fire Extinguisher Missing | High | Open | Facilities |
| Wet Floor Hazard | Medium | Open | Operations |
| Equipment Malfunction | Critical | In Progress | Maintenance |
| First Aid Kit Empty | Low | Open | Facilities |
| Broken Handrail | High | Open | Maintenance |

## ?? Configuration

### Database
- **Type**: SQLite
- **File**: `Datwise.Api/datwise-dev.db`
- **Auto-Initialize**: Yes, on API startup
- **Schema**: `datwise-dev.db` created with Issues table

### API Configuration
```json
// appsettings.Development.json
{
  "ApiBaseUrl": "http://localhost:53487"
}
```

### WebForms Configuration
```json
// appsettings.Development.json
{
  "ApiBaseUrl": "http://localhost:53487"
}
```
*Note: Uses HTTP for development, HTTPS for production*

## ?? Security & Performance

### Security
- Input validation on all fields
- SQL injection protection via EF Core
- HTTPS in production
- Proper error handling

### Performance
- Database indexes on Status, Severity, ReportedDate
- Async/await for all I/O
- EF Core query optimization
- Lazy loading disabled to prevent N+1 queries

## ??? Development Workflow

### Adding a New Feature
1. Create DTO in `Datwise.Contracts`
2. Add repository method in `Datwise.Data`
3. Add service method in `Datwise.Services`
4. Expose via `IssuesController` endpoint
5. Add unit tests in `Datwise.Tests`
6. Update UI in `Datwise.WebForms`

### Modifying API Behavior
1. Update `IIssueService` interface
2. Implement in `IssueService`
3. Update `IIssueRepository` if needed
4. Update controller endpoint
5. Add/update tests

## ?? Deployment

### Development
```bash
dotnet run              # Uses launchSettings.json
http://localhost:53488  # WebForms
http://localhost:53487  # API
```

### Production
- Set `ASPNETCORE_ENVIRONMENT=Production`
- Configure valid HTTPS certificates
- Use production database (SQL Server recommended)
- Update `ApiBaseUrl` in appsettings
- Enable CORS if needed
- Configure logging and monitoring

## ?? Troubleshooting

### Port Already in Use
```powershell
.\cleanup-ports.ps1
```

### Database Not Creating
1. Delete `Datwise.Api/datwise-dev.db`
2. Restart API
3. Check console for initialization messages

### SSL Certificate Issues (Development)
- API automatically uses HTTP for localhost in dev
- WebForms uses HTTP in development
- Use `http://localhost:53488` in browser

### Form Submission Fails
1. Verify API is running on port 53487
2. Check appsettings.json ApiBaseUrl
3. Review console logs for HTTP errors
4. Test API with cURL: `curl http://localhost:53487/api/issues`

## ?? Code Examples

### Get Issues with Filtering
```csharp
var response = await _httpClient.GetAsync(
    "http://localhost:53487/api/issues?status=Open,In Progress&sort=-date");
var issues = JsonSerializer.Deserialize<List<IssueViewModel>>(
    await response.Content.ReadAsStringAsync());
```

### Create New Issue
```json
POST http://localhost:53487/api/issues
Content-Type: application/json

{
  "title": "Safety Concern",
  "description": "Issue details",
  "severity": "High",
  "status": "Open",
  "reportedBy": "John Doe",
  "department": "Operations",
  "location": "Building A"
}
```

### Filter Multiple Statuses
```bash
curl "http://localhost:53487/api/issues?status=Open,In%20Progress&severity=High,Critical"
```

## ?? Support

For issues:
1. Check console output for detailed error messages
2. Verify API and WebForms are both running
3. Ensure database file exists: `Datwise.Api/datwise-dev.db`
4. Review test files for usage examples

## ?? License

MIT License - See LICENSE file for details

---

## ?? Future Work & Roadmap

### v1.0 MVP (Current - In Development)

**Current Features:**
- ? Single user (owner) - no login required
- ? View all open issues with real-time statistics
- ? Sort by all columns (9 sortable fields)
- ? Filter by status and severity (multi-value)
- ? Submit new issues via form
- ? View issue details with all fields
- ? Dark/Light theme toggle

**v1.0 Planned Features (Before Release):**
- ?? **Issue Editing** - Edit form to modify issue fields and submit PUT requests
  - Allow changing title, description, severity, department, location
  - Mark issue as resolved with resolution notes
  - Accessible to owner only
  
- ?? **Advanced Filtering UI** - Grid-based filtering interface
  - Click column headers to filter (dropdown for enums, textbox for text)
  - Multiple simultaneous filters with single sort
  - Visual indicators showing active filters
  - "Clear Filters" button to reset to default view
  
- ?? **Enhanced Issue Fields**
  - Image upload capability for issues
  - Additional metadata fields (urgency, impact, assigned to, etc.)
  - Rich text description support
  
- ?? **Form Validation & Security**
  - XSS (Cross-Site Scripting) prevention on all inputs
  - Field validation for meaningful data entry
  - Rate limiting on API endpoints
  - Input sanitization throughout
  - CSRF token validation (already implemented)

---

### v2.0 MVP (Authentication & Role-Based Access)

**Authentication:**
- Login page with username/password authentication
- Password hashing and encryption in database
- Session management
- User profiles with assigned roles

**User Roles:**
- **Owner** (1 user max) - Full access to all features
- **Manager** (multiple) - Can manage issues, view statistics, access logs
- **Viewer** (multiple) - Read-only access to issues
- **Guest** - Login without password, view-only access to limited data

**New Features:**
- Role-based access control (RBAC) on all endpoints
- Differentiated views based on user role
- Enhanced statistics panel with role-based filtering
- **Activity Logging** page (Owner/Manager only)
  - Track all actions performed on the system
  - View user activity history
  - Filter logs by date range, user, action type
  - Export logs for compliance

**API Changes:**
- Authentication endpoints (login, logout, refresh token)
- Authorization headers on all protected endpoints
- Audit logging on create/update/delete operations

---

### v3.0 MVP (Real-Time Updates & Advanced Export)

**Real-Time Features:**
- WebSocket integration for live updates
- Auto-refresh grid when new issues are reported
- Auto-update status changes without page reload
- Maintains current filters while receiving updates

**Advanced Export:**
- Export to Excel with formatting
- Filter capabilities before export
  - Date range filtering
  - Severity level selection
  - Status filtering
  - Department filtering
- Include charts and statistics in export

**Offline Capability:**
- Local storage of failed issue submissions
- Queue management for offline issues
- Automatic retry when connection restored
- User notification of pending submissions
- Conflict resolution if issue modified before retry

**Internationalization (i18n):**
- Complete English/Hebrew language support
- RTL (Right-to-Left) layout support for Hebrew
- Language toggle in UI
- Localized date/time formatting
- Translated error messages and labels

---

### v4.0 MVP (Modernization & AI)

**Modern UI & Mobile Support:**
- Migration from WebForms to modern framework (React/Vue/Angular)
- Native or responsive mobile app (iOS/Android)
- Progressive Web App (PWA) support
- Modern design system and UX improvements
- Existing WebForms maintained for transition period

**AI Assistant:**
- Natural language issue reporting
- Chat interface for creating issues
- Voice-activated support
- Smart field population from description
- Issue categorization and severity suggestion
- Duplicate detection

**Advanced Analytics:**
- New Analytics page with comprehensive dashboards
- Trend analysis (issues over time)
- Severity distribution charts
- Department-wise issue breakdown
- Mean time to resolution (MTTR) metrics
- SLA compliance tracking
- Historical data analysis
- Custom report builder
- Data export in multiple formats (CSV, PDF, Excel)

---

## ?? Detailed Feature Specifications (v1.0)

### Issue Editing
```
GET  /api/issues/{id}              # Already implemented
PUT  /api/issues/{id}              # Update issue with new details
                                   # Allowed fields: title, description, 
                                   # severity, status, department, location,
                                   # resolutionNotes, resolvedBy, resolvedDate

Fields editable by owner:
- Title
- Description
- Severity (Low, Medium, High, Critical)
- Status (Open, In Progress, Resolved)
- Department
- Location
- Resolution Notes (when closing issue)
```

### Advanced Filtering
```
Features:
- Click on column header ? open filter dialog
- Enum fields (severity, status) ? dropdown select
- Text fields (title, department, location, reportedBy) ? text input
- Multiple simultaneous filters (AND logic)
- Single sort option (ascending/descending)
- Active filters marked with [filter icon] on header
- "Clear Filters" button restores default (Open + In Progress status)

Example filtered state:
- Status: Open, In Progress
- Severity: High, Critical
- Department: Facilities
- Sort: Date (descending)
```

### Form Validation & Security
```
Input Validation:
- Max length enforcement
- Required field validation
- Email validation (if added)
- URL validation for images
- Numeric range validation

Security Measures:
- XSS prevention via output encoding
- SQL injection prevention (EF Core parameterization)
- CSRF token validation
- Rate limiting (TBD - max requests per minute)
- Content Security Policy (CSP) headers
- Secure headers (HSTS, X-Frame-Options, etc.)
```

---

## ??? Release Timeline (Estimated)

| Version | Timeline | Status |
|---------|----------|--------|
| v1.0 | Q1 2026 | In Development |
| v2.0 | Q2 2026 | Planned |
| v3.0 | Q3 2026 | Planned |
| v4.0 | Q4 2026+ | Conceptual |

---

## ?? Team & Development Guidelines

### Team Structure by Phase

**v1.0 Phase (Q1 2026):**
- **Team Size:** 1 Developer
- **Effort:** ~3 months (full-time equivalent)
- **Scope:** Single user interface, core CRUD operations, filtering, sorting, statistics
- **Parallel Activity:** During v1 development, plan v2 architecture and begin onboarding second developer

**v2.0 Phase (Q2 2026):**
- **Team Size:** 2 Developers
- **Effort:** 1 quarter (3 months) with 2 developers
- **Dev 1:** Backend enhancements - Authentication system, authorization, database schema updates, audit logging
- **Dev 2:** Frontend & API - Role-based views, logging page, enhanced statistics API
- **Coordination:** Weekly sync meetings, shared API contract documentation

**v3.0 Phase (Q3 2026):**
- **Team Size:** 2 Developers
- **Effort:** 1 quarter (3 months) with 2 developers
- **Dev 1:** Backend - WebSocket infrastructure, offline queue system, export service, internationalization backend
- **Dev 2:** Frontend - WebSocket client implementation, export UI, offline handling UI, i18n integration
- **Critical:** Begin hiring/onboarding 3rd developer by mid-Q3
- **Decision Point:** Select frontend framework for v4 modernization (React/Vue/Angular)

**v4.0 Phase (Q4 2026+):**
- **Team Size:** 3 Developers (or 2 if all are frontend-proficient)
- **Option A - Specialized Team:**
  - **Frontend Developer:** New hire, React/Vue/Angular expert
  - **Backend Developer 1:** API, AI integration, analytics backend
  - **Backend Developer 2:** Database optimization, performance, infrastructure
  
- **Option B - Full-Stack Team (Alternative):**
  - All 3 developers proficient in selected frontend framework
  - Frontend/UI development divided evenly
  - Backend work rotated or specialized by component
  - **Requirement:** All team members must be trained in selected framework by end of Q3

---

## ?? Onboarding & Knowledge Transfer

### v1 to v2 Transition (During Q1)
- **Week 1-2 of v1:** Second developer reviews codebase, architecture, and database design
- **Week 3-4 of v1:** Second developer contributes small features/bug fixes to get familiar with workflow
- **Documentation needed:** Architecture decision records (ADRs), setup guide, coding standards
- **Knowledge transfer:** Code reviews, pair programming sessions, technical documentation

### v2 to v3 Transition (During Q2)
- Third developer onboarding should begin
- Frontend framework selection and initial setup
- Prototype WebSocket implementation
- Shared documentation on new technologies

### v3 to v4 Transition (During Q3)
- **If hiring new Frontend Developer:** 4-week intensive onboarding on codebase, existing features, and frontend framework
- **If upskilling existing team:** 6-week training program on selected framework (React/Vue/Angular)
- **Parallel work:** 1-2 backend developers continue v3 stabilization while others train

---

## ?? Development Best Practices

### Code Quality Standards
- Minimum 80% unit test coverage
- Peer code reviews before merging (2+ approvals)
- Static code analysis using SonarQube or similar
- Automated builds and tests on every commit

### Documentation Requirements
- **Code:** XML documentation on all public methods
- **Architecture:** Keep ADRs (Architecture Decision Records) updated
- **API:** Swagger/OpenAPI documentation maintained
- **Database:** Entity-relationship diagrams and schema documentation
- **Processes:** Runbooks for deployment, troubleshooting, and common tasks

### Version Control Strategy
```
Branches:
- main          # Production releases only (v1.0, v2.0, etc.)
- develop       # Integration branch for current phase
- feature/*     # Individual feature branches
- hotfix/*      # Emergency production fixes
```

### Sprint Planning
- **2-week sprints** for better planning and coordination
- Weekly sync meetings between developers
- Daily standups (15 min max)
- Sprint retrospectives to identify improvements

---

## ?? Hiring Timeline

| Phase | Timeline | Role | Responsibility |
|-------|----------|------|-----------------|
| v1 | Q1 2026 | Backend Dev | Core platform development |
| v1-v2 transition | Week 12 of Q1 | Backend Dev #2 | Onboarding, v2 planning |
| v3-v4 transition | Mid-Q3 2026 | Frontend Dev or Full-Stack | React/Vue/Angular, modern UI |

### Ideal Candidate Profiles

**Backend Developer (v1):**
- 3+ years .NET experience
- Entity Framework Core proficiency
- API design and security background
- Full-stack capability preferred

**Backend Developer #2 (v2):**
- 2+ years .NET experience
- Authentication/Authorization knowledge
- Willing to learn WebSocket technologies
- Team player with communication skills

**Frontend Developer (v4) - Option A:**
- 3+ years React/Vue/Angular experience
- TypeScript proficiency
- Mobile-first responsive design
- UI/UX sensitivity

**Full-Stack Developers (v4) - Option B:**
- All team members must be trained on selected frontend framework
- Each developer mentors others in their area of expertise
- Emphasis on code sharing and pair programming

---

## ?? Resource Allocation

### Developer Capacity by Phase
```
v1.0:  1 dev × 13 weeks = 13 dev-weeks
v2.0:  2 devs × 13 weeks = 26 dev-weeks
v3.0:  2 devs × 13 weeks = 26 dev-weeks
v4.0:  3 devs × ongoing = flexible
```

### Budget Considerations
- Developer salaries (3 developers by Q4)
- Training budget for new framework ($5-10K per developer)
- Development tools and licenses
- Infrastructure costs (testing servers, deployment)
- Contingency for unforeseen complexities

---
