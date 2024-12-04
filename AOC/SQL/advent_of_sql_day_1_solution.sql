select
	FORMAT('%s,%s,%s,%s,%s,%s,%s',
		c.name,
		w.wishes->>'first_choice',
		w.wishes->>'second_choice',
		w.wishes->'colors'->>0,
		json_array_length(w.wishes->'colors'),
		CASE
		  WHEN t.difficulty_to_make = 1 THEN 'Simple Gift'
		  WHEN t.difficulty_to_make = 2 THEN 'Moderate Gift'
		  WHEN t.difficulty_to_make >= 3 THEN 'Complex Gift'
		END, 
		CASE
		  WHEN t.category = 'outdoor' THEN 'Outside Workshop'
		  WHEN t.category = 'educational' THEN 'Learning Workshop'
		ELSE 
			'General Workshop'
		END)
	as outputs,
	c.name,
	w.wishes->>'first_choice' as primary_wish,
	w.wishes->>'second_choice' as backup_wish,
	w.wishes->'colors'->>0 as favorite_color,
	json_array_length(w.wishes->'colors') as color_count,
	CASE
	  WHEN t.difficulty_to_make = 1 THEN 'Simple Gift'
	  WHEN t.difficulty_to_make = 2 THEN 'Moderate Gift'
	  WHEN t.difficulty_to_make >= 3 THEN 'Complex Gift'
	END as gift_complexity, 
	CASE
	  WHEN t.category = 'outdoor' THEN 'Outside Workshop'
	  WHEN t.category = 'educational' THEN 'Learning Workshop'
	ELSE 
		'General Workshop'
	END as workshop_assignment
from wish_lists as w
	left join children as c using(child_id)
	left join toy_catalogue as t on t.toy_name = w.wishes->>'first_choice'
order by c.name
limit 5;