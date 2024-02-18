CREATE UNIQUE INDEX uq_todo_status_todo_id_occur_date
ON public.todo_status (todo_id, occur_date);