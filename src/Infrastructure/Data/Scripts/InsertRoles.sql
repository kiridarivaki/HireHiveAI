DO $$ 
DECLARE
    role_name TEXT;
    role_id UUID;
BEGIN
    -- Insert Admin role if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM "AspNetRoles" WHERE "NormalizedName" = 'ADMIN') THEN
        role_id := gen_random_uuid();
        INSERT INTO "AspNetRoles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp")
        VALUES (role_id, 'Admin', 'ADMIN', gen_random_uuid());
    END IF;

    -- Insert Candidate role if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM "AspNetRoles" WHERE "NormalizedName" = 'CANDIDATE') THEN
        role_id := gen_random_uuid();
        INSERT INTO "AspNetRoles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp")
        VALUES (role_id, 'Candidate', 'CANDIDATE', gen_random_uuid());
    END IF;
END $$;
