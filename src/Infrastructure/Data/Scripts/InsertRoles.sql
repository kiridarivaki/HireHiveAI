DO $$
DECLARE
    role_name TEXT;
    role_id UUID;
BEGIN
    FOREACH role_name IN ARRAY @roles
    LOOP
        SELECT 1 FROM "AspNetRoles" WHERE "NormalizedName" = UPPER(role_name)
        role_id := gen_random_uuid(); 
        INSERT INTO "AspNetRoles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp")
        VALUES (role_id, role_name, UPPER(role_name), gen_random_uuid());
    END LOOP;
END $$;
