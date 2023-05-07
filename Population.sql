USE Fleurs;

# Table address
INSERT INTO address (name, complement, street, city, postal_code) VALUES
('Maison', 'Etage 4 Porte 7', '146 rue salvador allende', 'Nanterre', '92000'),
('Maison','','3 allée sans fibre', 'Villescrenes', '94440'),
('Appartement', '', '24 rue des Lilas', 'Lyon', '69003'),
('Bureau', 'Bâtiment A, 2ème étage', '15 avenue de la Grande Armée', 'Paris', '75016'),
('Maison', '', '7 rue des Chênes', 'Toulouse', '31000'),
('Appartement', 'Résidence Les Acacias', '14 avenue de la République', 'Lille', '59000'),
('Maison', '', '20 rue de la Paix', 'Bordeaux', '33000'),
('Bureau', 'Immeuble Le Méridien, 7ème étage', '28 Boulevard Haussmann', 'Paris', '75009'),
('Appartement', 'Batiment B', '2 rue de la Paix', 'Paris', '75001'),
('Maison', '', '10 rue des Lilas', 'Bordeaux', '33000');

# Table credit_card
INSERT INTO credit_card (name, holder, number, expiry, ccv) VALUES 
('Carte personnel', 'TOURNEUX Elias', '4974123499872048', '2025-04-01', '266'),
('Carte personnel', 'DUPONT Jean', '5475103521453097', '2024-11-01', '123'),
('Carte travail', 'MARTIN Claire', '4539589325018486', '2026-08-01', '789'),
('Carte voyage', 'LAMBERT Lucie', '5228768214759061', '2023-06-01', '546'),
('Carte achats', 'GARCIA Carlos', '4716464245270436', '2024-09-01', '321'),
('Carte loisirs', 'LEFEBVRE Amandine', '4916572205823940', '2025-03-01', '984'),
('Carte crédit', 'FOURNIER Simon', '4024007110675432', '2026-01-01', '357'),
('Carte entreprise', 'PETIT Laurent', '4485854285736902', '2023-12-01', '690'),
('Carte personnelle', 'THOMAS Marie', '4929531359455236', '2025-07-01', '132'),
('Carte loisirs', 'RENAUD Thomas', '5468321914329156', '2024-02-01', '657');

# Table clients
INSERT INTO clients (name, surname, phone, email, password, address, credit_card, employee) VALUES
('Elias', 'Tourneux', '0661531991', 'me@eliqs.dev', 'toutou&67', 1, 1, 0),
('Marc', 'Viallard', '0645123456', 'marc.viallard@hotmail.com', 'marximarc*$4442', 2,2, 0),
('Emma', 'Durand', '0678563214', 'emma.durand@gmail.com', 'passw0rd', 3, 3, 0),
('Lucie', 'Lefebvre', '0687456321', 'lucie.lefebvre@hotmail.com', '123456789', 4, 4, 0),
('Thomas', 'Garcia', '0654789123', 'thomas.garcia@outlook.fr', 'qwerty', 5, 5, 0),
('Antoine', 'Lemoine', '0645123789', 'antoine.lemoine@yahoo.com', 'azerty', 6, 6, 0),
('Clara', 'Renard', '0612345678', 'clara.renard@laposte.net', 'coucou123', 7, 7, 0),
('Julie', 'Besson', '0678451297', 'julie.besson@gmail.com', 'toto123', 8, 8, 0),
('Adrien', 'Morel', '0645789321', 'adrien.morel@hotmail.com', 'p@ssword', 9, 9, 0),
('Sophie', 'Dubois', '0612345689', 'sophie.dubois@yahoo.com', 'password123', 10, 10, 0);

# Table products

INSERT INTO products (name, description, price, stock, alert) VALUES
('Vase en verre', 'Un vase en verre élégant et raffiné.', '20.00', 50, 0),
('Boîte-cadeau', 'Une boîte-cadeau de qualité pour offrir vos fleurs.', '10.00', 100, 0),
('Ruban', 'Un ruban en satin pour parfaire la présentation de vos bouquets.', '5.00', 200, 0),
('Carte de vœux', 'Une carte de vœux pour accompagner votre bouquet.', '3.00', 500, 0),
('Chocolats', 'Des chocolats fins pour accompagner votre bouquet.', '15.00', 100, 0);

# Table flowers

INSERT INTO flowers (name, price, start_month, end_month, stock, alert) VALUES
('Gerbera', '5.00', 1, 12, 7, 0),
('Ginger', '4.00', 1, 12, 6, 0),
('Glaïeul', '1.00', 5, 11, 10, 0),
('Marguerite', '2.25', 1, 12, 10, 0),
('Rose rouge', '2.50', 1, 12, 10, 0);

# Table category

INSERT INTO category (name) VALUES
('Toute occasion'),
('St-Valentin'),
('Fête des mères'),
('Mariage');

# Table bouquets

INSERT INTO bouquets (name, description, price, category, stock, alert) VALUES
('Gros Merci', 'Arrangement floral avec marguerites et verdure', '45.00', 1, 9, 1),
('L’amoureux', 'Arrangement floral avec roses blanches et roses rouges', '65.00', 2, 1, 0),
('L’Exotique', 'Arrangement floral avec ginger, oiseaux du paradis, roses et genet', '40.00', 1, 4, 0),
('Maman', 'Arrangement floral avec gerbera, roses blanches, lys et alstroméria', '80.00', 3, 5, 0),
('Vive la mariée', 'Arrangement floral avec lys et orchidées', '120.00', 4, 5, 0);

# Table shops
INSERT INTO shops (city) VALUES
('Strasbourg'),
('Lyon'),
('Paris'),
('Marseille'),
('Bordeaux'),
('Lille'),
('Toulouse'),
('Nantes'),
('Nice'),
('Montpellier');

# Table orders
INSERT INTO orders (client_id, type, wishes, max_price, address, message, delivery, creation_date, status, shop) VALUES
(1, 'CP', 'Bouquet de roses rouges', '40.00', 1, 'Livraison avant 14h', '2023-05-15', '2023-05-06 16:59:07', 'VINV', 1),
(2, 'CS', 'Composition florale pour mariage', '120.00', 2, 'Pas de livraison le lundi', '2023-05-18', '2023-05-06 16:59:07', 'VINV', 2),
(3, 'CP', 'Bouquet de pivoines', '50.00', 3, '', '2023-05-17', '2023-05-06 16:59:07', 'VINV', 3),
(4, 'CS', 'Bouquet de fleurs des champs', '30.00', 4, '', '2023-05-16', '2023-05-06 16:59:07', 'VINV', 4),
(5, 'CP', 'Roses blanches pour anniversaire', '45.00', 5, '', '2023-05-20', '2023-05-06 16:59:07', 'VINV', 5),
(6, 'CS', 'Bouquet de tulipes multicolores', '35.00', 6, 'Attention au chien dans la cour', '2023-05-19', '2023-05-06 16:59:07', 'VINV', 6),
(7, 'CP', 'Bouquet de roses blanches', '40.00', 7, '', '2023-05-22', '2023-05-06 16:59:07', 'VINV', 7),
(8, 'CS', 'Composition florale pour baptême', '100.00', 8, '', '2023-05-23', '2023-05-06 16:59:07', 'VINV', 8),
(9, 'CP', 'Bouquet de lys', '55.00', 9, '', '2023-05-24', '2023-05-06 16:59:07', 'VINV', 9),
(10, 'CS', 'Bouquet de roses et tulipes', '35.00', 10, '', '2023-05-25', '2023-05-06 16:59:07', 'VINV', 10);

# Table order_flowers
INSERT INTO order_flowers (order_id, flower_id, quantity) VALUES
(1, 1, 3),
(1, 2, 2),
(2, 2, 4),
(2, 4, 1),
(3, 5, 2),
(4, 1, 5),
(4, 3, 3),
(5, 4, 2),
(6, 2, 2),
(6, 3, 1),
(7, 1, 2),
(7, 2, 5),
(8, 4, 1),
(8, 1, 2),
(9, 2, 5),
(9, 4, 1),
(10, 1, 1),
(10, 2, 3),
(10, 3, 2),
(10, 4, 1),
(10, 5, 2);

# Table order_products
INSERT INTO order_products (order_id, product_id, quantity) VALUES
(1, 1, 3),
(1, 2, 2),
(2, 2, 4),
(2, 4, 1),
(3, 5, 2),
(4, 1, 5),
(4, 3, 3),
(5, 4, 2),
(6, 2, 2),
(6, 3, 1),
(7, 1, 2),
(7, 2, 5),
(8, 4, 1),
(8, 1, 2),
(9, 2, 5),
(9, 4, 1),
(10, 1, 1),
(10, 2, 3),
(10, 3, 2),
(10, 4, 1),
(10, 5, 2);

# Table order_bouquets
INSERT INTO order_bouquets (order_id, bouquet_id, quantity) VALUES
(1, 1, 3),
(1, 2, 2),
(2, 2, 4),
(2, 4, 1),
(3, 5, 2),
(4, 1, 5),
(4, 3, 3),
(5, 4, 2),
(6, 2, 2),
(6, 3, 1),
(7, 1, 2),
(7, 2, 5),
(8, 4, 1),
(8, 1, 2),
(9, 2, 5),
(9, 4, 1),
(10, 1, 1),
(10, 2, 3),
(10, 3, 2),
(10, 4, 1),
(10, 5, 2);



