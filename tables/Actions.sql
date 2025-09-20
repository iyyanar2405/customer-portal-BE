CREATE TABLE Actions (
    id INT PRIMARY KEY,
    action NVARCHAR(255),
    dueDate DATETIME,
    highPriority BIT,
    message NVARCHAR(MAX),
    language NVARCHAR(50),
    service NVARCHAR(100),
    site NVARCHAR(100),
    entityType NVARCHAR(100),
    entityId INT,
    subject NVARCHAR(255),
    snowLink NVARCHAR(255)
);



CREATE TABLE Audits (
    auditId INT PRIMARY KEY,
    companyId INT,
    status NVARCHAR(50),
    startDate DATETIME,
    endDate DATETIME,
    leadAuditor NVARCHAR(100),
    type NVARCHAR(50)
);

CREATE TABLE AuditSites (
    auditId INT,
    siteId INT,
    FOREIGN KEY (auditId) REFERENCES Audits(auditId)
);

CREATE TABLE AuditServices (
    auditId INT,
    service NVARCHAR(100),
    FOREIGN KEY (auditId) REFERENCES Audits(auditId)
);