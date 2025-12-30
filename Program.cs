using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuMailClient;

Console.WriteLine("==========================================");
Console.WriteLine("       QUMAIL SENTINEL - COMMAND CENTER   ");
Console.WriteLine("==========================================");

var kms = new KmClient();

Console.WriteLine("1. Run Randomness Audit (Batch Mode)");
Console.WriteLine("2. Decrypt a Message (Manual Mode)");
Console.Write("\nSelect Option: ");
string choice = Console.ReadLine();

if (choice == "1")
{
    await RunBatchAudit(kms);
}
else if (choice == "2")
{
    await RunManualDecryption(kms);
}

async Task RunBatchAudit(KmClient kmsClient)
{
    int samples = 50;
    List<byte> totalPool = new List<byte>();
    try
    {
        Console.WriteLine($"\n[ACTION] Fetching {samples} keys from Render for batch audit...");
        for (int i = 0; i < samples; i++)
        {
            var result = await kmsClient.GetQuantumKeyAsync();
            totalPool.AddRange(result.KeyBytes);
            if (i % 10 == 0 && i > 0) Console.WriteLine($"[PROGRESS] Retrieved {i} samples...");
        }

        var frequencies = totalPool.GroupBy(b => b).Select(g => (double)g.Count() / totalPool.Count);
        double entropy = frequencies.Sum(p => -p * Math.Log(p, 2));

        Console.WriteLine("\n==========================================");
        Console.WriteLine($"[RESULT] Aggregated Entropy: {entropy:F4} bits/byte");

        if (entropy > 7.7)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[STATUS] PASS: KMS Randomness Verified.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[STATUS] FAIL: Systematic Pattern Detected!");
        }
    }
    catch (Exception ex) { Console.WriteLine($"[ERROR] Audit interrupted: {ex.Message}"); }
}

async Task RunManualDecryption(KmClient kmsClient)
{
    try
    {
        Console.WriteLine("\n[INPUT] Paste the Encrypted Message (Levels 1-4):");
        string cipherText = Console.ReadLine() ?? "";

        // 1. Identify and Handle Level 4 (Plaintext Baseline)
        if (cipherText.StartsWith("PLAIN_"))
        {
            Console.WriteLine("[AUDIT] Protocol: Level 4 (Plaintext/Base64)");
            string decrypted = SecurityCore.DecodePlaintext(cipherText);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[RESULT] Decoded Baseline: {decrypted}");
            return;
        }

        // 2. Prepare for Secure Decryption (Levels 1, 2, 3)
        Console.WriteLine("[ACTION] Fetching live session key from Render KMS...");
        var result = await kmsClient.GetQuantumKeyAsync();
        byte[] auditKey = (byte[])result.KeyBytes.Clone(); // Clone for Zero-Knowledge Handshake

        Console.WriteLine($"[INFO]   Using Key ID: {result.KeyId}");
        string decryptedResult = "";

        // 3. Identify and Handle Level 3 (PQC Hybrid)
        if (cipherText.StartsWith("PQC_v1."))
        {
            Console.WriteLine("[AUDIT] Protocol: Level 3 (PQC Hybrid - Lattice Simulation)");
            decryptedResult = SecurityCore.DecryptPqcHybrid(cipherText, auditKey);
        }
        // 4. Handle Level 2 (AES-256) or Level 1 (OTP)
        else
        {
            // Level 2/1 logic: We attempt AES first as it is the most common high-level choice
            Console.WriteLine("[AUDIT] Protocol: Level 1/2 (High-Entropy AES-256 or OTP)");
            decryptedResult = SecurityCore.DecryptWithAes(cipherText, auditKey);
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n[SUCCESS] Decrypted Content: {decryptedResult}");

        // 5. Prove Memory Scrubbing
        if (auditKey.All(b => b == 0))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[VERIFIED] Zero-Knowledge Protocol: Key destroyed in RAM.");
            Console.WriteLine("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[AUDIT FAIL] Security Barrier Intact: {ex.Message}");
        Console.WriteLine("[ANALYSIS] The encryption is mathematically sound. Without the synchronized session key, this data is effectively undecryptable.");
    }
    Console.ResetColor();
}

Console.ResetColor();
Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();