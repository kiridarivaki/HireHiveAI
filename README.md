# Hire Hive: AI-Powered Recruitment Support System

## Project Overview

This repository contains the source code for **Hire Hive**, a web-based recruitment assistance platform designed to enhance efficiency and objectivity in candidate evaluation through the strategic use of Artificial Intelligence. Hire Hive aims to support recruiters in identifying suitable candidates by automating resume assessment and providing AI-generated match evaluations based on defined job criteria.

## Key Features

* **User Registration & Profile Management:** Secure user registration with role-based access control. Candidates can manage profiles and upload resumes.
* **Recruiter Job Posting & Criteria Definition:** Recruiters can define detailed job descriptions and custom assessment criteria with adjustable weightings.
* **AI-Powered Resume Assessment Module:** On-demand assessment of candidate resumes, returning percentage matches and AI-generated summaries of strengths/weaknesses. Supports incremental loading and sorting of results.
* **Bias Mitigation Strategy:** A preprocessing pipeline leveraging Azure AI services removes Personally Identifiable Information (PII) from resumes before assessment to reduce algorithmic bias.
* **Resume Preview & Download:** Recruiters can preview and download original candidate resumes.

## Technology Stack

Hire Hive is built with a modern, scalable, and maintainable technology stack:

* **Backend/API Layer:** ASP.NET Core Web API 9 (Clean Architecture)
* **Frontend:** Angular
* **Database:** PostgreSQL (via Supabase)
* **Hosting:** Azure App Services (Azure Web App for backend, Azure Static Web App for frontend)
* **AI Services:** Azure Document Intelligence, Azure AI Language (PII Detection), Azure AI Model Inference API (for ChatGPT 4.1)
* **CI/CD:** GitHub Actions
* **Authentication:** JWT-based authentication
* **Background Jobs:** Hangfire

## System Architecture

The application follows a modular architecture:

* **Backend (Clean Architecture):** Organized into Domain, Application, Infrastructure, and API layers, promoting separation of concerns and maintainability.
* **Frontend (Angular):** Structured with an emphasis on component reusability, lazy loading modules, and a feature-based architecture.

## Live Application & Demo

* **Deployed Application:** [https://wonderful-flower-0782c0b03.6.azurestaticapps.net/](https://wonderful-flower-0782c0b03.6.azurestaticapps.net/)
* **Demo Video:** [https://drive.google.com/file/d/1RkgUHDOcjqK_3wYff1oKdVZznZa07DQC/view](https://drive.google.com/file/d/1RkgUHDOcjqK_3wYff1oKdVZznZa07DQC/view)
