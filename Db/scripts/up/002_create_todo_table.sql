CREATE TABLE public.todo
(
    id         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    name       VARCHAR(255) NOT NULL,
    created_at timestamptz  NOT NULL,
    updated_at timestamptz,
    parent_id  uuid,
    user_id  uuid NOT NULL,
    CONSTRAINT fk_parent FOREIGN KEY (parent_id) REFERENCES public.todo (id) ON DELETE CASCADE,
    CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES public.user (id),
    CONSTRAINT user_name_parent_uni UNIQUE NULLS NOT DISTINCT (user_id, name, parent_id)
)