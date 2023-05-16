
CREATE TABLE IF NOT EXISTS "Url"(
    id bigserial NOT NULL,
    "md5" varchar(32) NOT NULL,
    "original_url" text default NOT NULL,
    "short_url" text default NOT NULL
)
