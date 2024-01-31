CREATE TABLE public.todo_status
(
    id           uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    is_completed BOOLEAN,
    occur_date   date        NOT NULL,
    completed_at timestamptz,
    todo_id      uuid        NOT NULL,
    created_at   timestamptz NOT NULL,
    updated_at   timestamptz,
    CONSTRAINT fk_todo FOREIGN KEY (todo_id) REFERENCES public.todo (id) ON DELETE CASCADE
)