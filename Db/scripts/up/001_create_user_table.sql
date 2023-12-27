CREATE TABLE public.user
(
    id         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    oid        VARCHAR(255) NOT NULL,
    email      VARCHAR(100) NOT NULL,
    created_at timestamptz  NOT NULL,
    updated_at timestamptz,
    UNIQUE (oid)
)