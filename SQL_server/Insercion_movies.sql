-- Insertar películas
INSERT INTO Movies (Name, Description, Director, Cast, ReleaseDate, Image)
VALUES 
(
  'Mad Max: Fury Road',
  'Una guerra en el desierto post-apocalíptico con acción desenfrenada.',
  'George Miller',
  'Tom Hardy, Charlize Theron',
  '2015-05-15',
  'madmax_furyroad.webp'
),
(
  'Deadpool',
  'Un ex-operativo de las fuerzas especiales se convierte en un antihéroe sarcástico.',
  'Tim Miller',
  'Ryan Reynolds, Morena Baccarin',
  '2016-02-12',
  'deadpool.webp'
),
(
  'John Wick',
  'Un asesino retirado regresa por venganza tras perder a su perro.',
  'Chad Stahelski',
  'Keanu Reeves, Michael Nyqvist',
  '2014-10-24',
  'johnwick.webp'
),
(
  'Mickey 17',
  'Mickey 17 sigue la historia de Mickey, un hombre que trabaja como ''explorador'' en una misión de colonización en un planeta distante.',
  'Bong Joon-ho',
  'Robert Pattinson, Naomi Ackie, Steven Yeun, Toni Collette',
  '2025-02-28',
  'mickey17.webp'
);

-- Verificar la inserción
SELECT * FROM Movies;