![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

QuMailSentinel // Quantum-Safe Communication Terminal
QuMailSentinel is a specialized secure messaging terminal developed to mitigate the risks of "Harvest Now, Decrypt Later" attacks. By decoupling the data transmission path from the key distribution path, the application ensures that intercepted email traffic remains mathematically useless without access to the specific Quantum Key Management System (KMS).

Core Security Protocols
The application features a modular security engine supporting four distinct levels of encryption:

Level 1: Quantum One-Time Pad (OTP) – Information-theoretic security using high-entropy keys provided by the KMS.

Level 2: Quantum AES-256 – Standard symmetric encryption utilizing 256-bit keys distributed via simulated Quantum Key Distribution (QKD).

Level 3: PQC Hybrid – A simulated Post-Quantum Cryptography wrapper (Lattice-based) over standard AES for future-proof resilience.

Level 4: Standard Plaintext – Unencrypted Base64 encoding used for baseline vulnerability demonstrations.

Key Features
Identity Persistence: Local storage of user SMTP credentials via Windows User Settings for a seamless multi-user experience.

Stealth UI: A custom-themed dark terminal interface with a hidden scroll-management system for a modern, professional look.

Forensic Verification: Includes a standalone "Interceptor" tool to demonstrate that Level 1-3 traffic is indistinguishable from noise without the session key.

Dynamic SMTP Integration: Powered by MailKit and MimeKit for secure transmission using user-generated App Passwords.

Technical Stack
Language: C# / .NET

UI Framework: WPF (Windows Presentation Foundation)

Libraries: MailKit, MimeKit, System.Security.Cryptography

Infrastructure: REST-based KMS Simulator for Quantum Key distribution

Setup & Configuration
KMS Node: Ensure the KMS Node Simulator is running on your local endpoint.

Identity: Go to Key Configuration and enter your Email and Gmail App Password.

Persistence: The application will remember your identity for future sessions.
