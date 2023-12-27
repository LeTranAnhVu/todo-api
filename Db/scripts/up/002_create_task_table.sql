CREATE TABLE public.task
(
    id         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    name       VARCHAR(255) NOT NULL,
    created_at timestamptz  NOT NULL,
    updated_at timestamptz,
    parent_id  uuid,
    user_id  uuid NOT NULL,
    CONSTRAINT fk_parent FOREIGN KEY (parent_id) REFERENCES public.task (id),
    CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES public.user (id)
)