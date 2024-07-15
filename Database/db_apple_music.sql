-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 15, 2023 at 09:57 AM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_apple_music`
--

-- --------------------------------------------------------

--
-- Table structure for table `detail_pesanan`
--

CREATE TABLE `detail_pesanan` (
  `id` int(11) NOT NULL,
  `pesanan_id` int(11) DEFAULT NULL,
  `kItem_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `detail_pesanan`
--

INSERT INTO `detail_pesanan` (`id`, `pesanan_id`, `kItem_id`) VALUES
(20, 24, 48),
(21, 24, 50);

-- --------------------------------------------------------

--
-- Table structure for table `kategori_produk`
--

CREATE TABLE `kategori_produk` (
  `id` int(11) NOT NULL,
  `nama_kategori` varchar(50) NOT NULL,
  `deskripsi` varchar(500) DEFAULT NULL,
  `imageUrl` varchar(255) NOT NULL,
  `status` bit(1) NOT NULL DEFAULT b'1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `kategori_produk`
--

INSERT INTO `kategori_produk` (`id`, `nama_kategori`, `deskripsi`, `imageUrl`, `status`) VALUES
(13, 'Drum', 'Drum adalah kelompok alat musik perkusi yang terdiri dari kulit yang direntangkan dan dipukul dengan tangan atau sebuah batang atau biasanya disebut Stick drum. Selain kulit, drum juga digunakan dari bahan lain, misalnya plastik. Drum terdapat di seluruh dunia dan memiliki banyak jenis, misalnya kendang, timpani, Bodhrán, Ashiko, snare drum, bass drum, tom-tom, beduk, dan lain-lain.\n\nDalam musik pop, rock, dan jazz, drums biasanya mengacu kepada drum kit atau drum set,', '0244bdb5-4b0e-4634-8642-afb6a6ac0dac.png', b'1'),
(14, 'Piano', 'Piano (yang juga disebut pianoforte) adalah alat musik tuts yang diklasifikasikan sebagai instrumen dawai dan perkusi yang dimainkan dengan menekan tuts-tuts pada papan piano. ', '29122e90-056f-47ca-91a4-1baef231e84f.png', b'1'),
(15, 'Gitar', 'Gitar adalah sebuah alat musik berdawai yang dimainkan dengan cara dipetik, umumnya menggunakan jari maupun plektrum. Gitar terbentuk atas sebuah bagian tubuh pokok dengan bagian leher yang padat seba', '5bf25fed-49cc-45f6-ad76-e26a9ab9ae13.png', b'1'),
(16, 'Bass', 'Gitar bas elektrik (biasa disebut gitar bas, bas elektrik atau bas saja) adalah alat musik dawai yang menggunakan listrik untuk memperbesar suaranya. Penampilannya mirip dengan gitar elektrik,', '63488f0b-f8ad-450e-9e9f-1964e5661af6.png', b'1'),
(17, 'Biola', 'Biola adalah sebuah alat musik dawai yang dimainkan dengan cara digesek. Biola memiliki empat senar (G-D-A-E) yang disetel berbeda satu sama lain dengan interval sempurna kelima. ', '3ec720f0-463c-41bb-adc9-38fa93149c1d.png', b'1'),
(18, 'Menyanyi', 'Penyanyi adalah seseorang yang membawakan sebuah lagu dengan cara mengeluarkan nada melodis melalui suara dari mulutnya baik dengan iringan musik maupun tidak.', '25798552-7a8e-40c6-b6ee-12297c6e53bd.png', b'1'),
(19, 'Flute', 'Seruling atau suling adalah alat musik dari keluarga alat musik tiup kayu atau terbuat dari bambu. Suara seruling berciri lembut dan dapat dipadukan dengan alat musik lainnya dengan baik', '6d2fc8ce-69a3-4ba8-823d-30f9914fd860.png', b'1'),
(20, 'Saxophone', 'Saksofon atau seksofon atau saksopon adalah alat musik tiup kayu yang terbuat dari kuningan, berbentuk seperti cangklong rokok, dan memiliki mulut tiup buluh tunggal.[2] Meski dibuat dari logam kuningan, alat musik ini tergolong alat musik tiup kayu karena suaranya dihasilkan dari mulut tiup yang dibuat dari kayu, bukan logam. Bersama dengan alat musik tiup kayu lainnya, nada-nada yang dibuat oleh saksofon diatur dengan menutup lubang pada tabung saksofon.[3] \r\n', '902a0d9c-4b66-4e2f-82cb-3d4ecd4accfe.png', b'1');

-- --------------------------------------------------------

--
-- Table structure for table `keranjang_item`
--

CREATE TABLE `keranjang_item` (
  `id` int(11) NOT NULL,
  `schedule` date NOT NULL,
  `status` bit(1) NOT NULL DEFAULT b'1',
  `pengguna_id` int(11) DEFAULT NULL,
  `produk_id` int(11) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `keranjang_item`
--

INSERT INTO `keranjang_item` (`id`, `schedule`, `status`, `pengguna_id`, `produk_id`, `created_at`) VALUES
(34, '2023-11-14', b'0', 61, 32, '2023-11-14 09:13:53'),
(36, '2023-11-14', b'0', 61, 33, '2023-11-14 09:13:53'),
(39, '2023-11-15', b'0', 61, 35, '2023-11-15 05:56:19'),
(40, '2023-11-15', b'0', 61, 33, '2023-11-15 05:56:19'),
(41, '2023-11-15', b'0', 61, 36, '2023-11-15 05:56:19'),
(42, '2023-11-15', b'0', 61, 32, '2023-11-15 05:57:21'),
(43, '2023-11-17', b'0', 61, 35, '2023-11-15 05:57:21'),
(44, '2023-11-20', b'0', 61, 33, '2023-11-15 06:32:24'),
(47, '2023-11-18', b'0', 61, 33, '2023-11-15 06:38:59'),
(48, '2023-11-15', b'0', 61, 34, '2023-11-15 08:45:26'),
(49, '2023-11-21', b'0', 61, 33, '2023-11-15 07:45:05'),
(50, '2023-11-17', b'0', 61, 33, '2023-11-15 08:45:27'),
(51, '2023-11-17', b'1', 61, 36, '2023-11-15 08:51:11'),
(52, '2023-11-20', b'1', 61, 35, '2023-11-15 08:51:21');

-- --------------------------------------------------------

--
-- Table structure for table `metode_pembayaran`
--

CREATE TABLE `metode_pembayaran` (
  `id` int(11) NOT NULL,
  `nama_metode` varchar(20) NOT NULL,
  `imageUrl` varchar(255) NOT NULL,
  `status` bit(1) NOT NULL DEFAULT b'1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `metode_pembayaran`
--

INSERT INTO `metode_pembayaran` (`id`, `nama_metode`, `imageUrl`, `status`) VALUES
(1, 'agus', '357a44b5-a1b7-48c0-85a7-579e0877bc52.png', b'1'),
(2, 'agus', '20f23e44-47d8-4d76-a071-00be7aa6539c.png', b'1'),
(4, 'Gopay', '357a44b5-a1b7-48c0-85a7-579e0877bc52.png', b'1'),
(5, 'DANA', '20f23e44-47d8-4d76-a071-00be7aa6539c.png', b'1'),
(6, 'OVO', 'f94d3c80-9824-4625-9884-25bb60168bd1.png', b'1'),
(7, 'BNI', '634b78e7-5c26-4cf8-9d7f-252577f40d9b.png', b'1'),
(8, 'BCA', '48fa6f74-08f5-4008-89d6-a5dad75b6eb6.png', b'1'),
(9, 'MANDIRI', 'f89273b9-d4ca-4ccf-b219-f3aae7176d2d.png', b'1');

-- --------------------------------------------------------

--
-- Table structure for table `pengguna`
--

CREATE TABLE `pengguna` (
  `id` int(11) NOT NULL,
  `nama` varchar(30) NOT NULL,
  `email` varchar(30) NOT NULL,
  `password` varchar(200) NOT NULL,
  `role` varchar(10) NOT NULL,
  `status` bit(1) NOT NULL DEFAULT b'1',
  `reset_password_token` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pengguna`
--

INSERT INTO `pengguna` (`id`, `nama`, `email`, `password`, `role`, `status`, `reset_password_token`) VALUES
(11, 'irsyad', 'irsyadmuhammad001@gmail.com', '159', 'admin', b'1', '48205CB45E22059925B6822787CF18CB085B16DA'),
(45, 'apel', 'apel@gmail.com', '414767634010A8DDB3B32C37617C2EB935747940', 'admin', b'1', NULL),
(48, 'string', 'string', 'ECB252044B5EA0F679EE78EC1A12904739E2904D', 'admin', b'1', NULL),
(61, 'irsyad', 'irsyadmuhammad003@gmail.com', '6B6277AFCB65D33525545904E95C2FA240632660', 'user', b'1', '00E9B966BCBAF7B90A951F86472D466700A29129'),
(62, 'a', 'a', '86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', 'admin', b'1', NULL),
(65, '', 'irsyadmuhammad339@gmail.com', '6B6277AFCB65D33525545904E95C2FA240632660', 'user', b'1', 'DAD5AC4633345DA96D48C63CC05BF2855131946F');

-- --------------------------------------------------------

--
-- Table structure for table `pesanan`
--

CREATE TABLE `pesanan` (
  `id` int(11) NOT NULL,
  `metode_id` int(11) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pesanan`
--

INSERT INTO `pesanan` (`id`, `metode_id`, `created_at`) VALUES
(24, 9, '2023-11-15 08:45:26');

-- --------------------------------------------------------

--
-- Table structure for table `produk`
--

CREATE TABLE `produk` (
  `id` int(11) NOT NULL,
  `nama_produk` varchar(50) NOT NULL,
  `deskripsi_produk` varchar(200) DEFAULT NULL,
  `harga` int(10) NOT NULL,
  `imageUrl` varchar(255) NOT NULL,
  `status` bit(1) NOT NULL DEFAULT b'1',
  `kategori_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `produk`
--

INSERT INTO `produk` (`id`, `nama_produk`, `deskripsi_produk`, `harga`, `imageUrl`, `status`, `kategori_id`) VALUES
(32, 'Kursus Drummer Special Coach (Eno Netral)', 'Kursus Drummer Special Coach (Eno Netral), dijamin langsung jago, bisa tour keliling dunia bersama eno netral', 850000, '7eee57f9-b13e-4a76-bee6-ceeee93250b7.png', b'1', 13),
(33, '[Beginner] Guitar class for kids', '[Beginner] Guitar class for kids\r\n[Beginner] Guitar class for kids', 1600000, '7b83ee8c-d907-46c2-8376-13983cc4bf23.png', b'1', 15),
(34, 'Biola Mid-Level Course', 'Biola Mid-Level Course', 3000000, '65e9968b-0708-47ae-bb01-4b5f6f8c456f.png', b'1', 17),
(35, 'Drummer for kids (Level Basic/1)', 'Drummer for kids (Level Basic/1)', 2200000, '3be2b062-0357-46aa-ab2d-ad25f28614cc.png', b'1', 13),
(36, 'Kursu Piano : From Zero to Pro (Full Package)', 'Kursu Piano : From Zero to Pro (Full Package)', 11650000, 'c86d4dfe-0fc6-45c4-a03f-b01e5b0c9ab6.png', b'1', 14),
(37, 'Expert Level Saxophone', 'Expert Level Saxophone', 7350000, '1a5b3a0d-e8f5-4755-8efa-dc26426df691.png', b'1', 20),
(38, 'Gendang sakti Gendang sakti Gendang sakti ', 'Gendang sakti Gendang sakti Gendang sakti ', 200000000, '1a5b3a0d-e8f5-4755-8efa-dc26426df691.png', b'1', 18),
(39, 'Expert Level Drummer Lessons', 'Expert Level Drummer Lessons\r\n', 5450000, 'd31f27e7-7e6b-4148-b572-0e71a4f8beb9.png', b'1', 13),
(40, 'From Zero to Professional Drummer ', 'From Zero to Professional Drummer (Complit Package)\r\n', 13000000, '61440092-49e1-4483-a885-49caa64394e4.png', b'1', 13),
(42, 'Kursus Singing Special Coach (Ariel Noah)', 'Kursus Singing Special Coach (Ariel Noah)\r\n', 2000000, '7eee57f9-b13e-4a76-bee6-ceeee93250b7.png', b'1', 18),
(43, '[Beginner] Bass class for teen', '[Beginner] Bass class for teen', 18250000, '28152804-1825-4267-844a-a53ab628507b.png', b'1', 16),
(44, 'GOD Level Flute', 'GOD Level Flute', 2500000, 'd60489ea-4a9a-46c1-82c1-77d373cd39c2.png', b'1', 19),
(45, 'Expert Level Flute', 'Expert Level Flute\r\n', 15000000, '29d3117e-6e71-41d1-944a-457639322ff6.png', b'1', 19),
(46, 'GOD Level Saxophone', 'GOD Level Saxophone', 45000000, 'c056193c-143d-4e5c-a700-4e1194863382.png', b'1', 20);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `detail_pesanan`
--
ALTER TABLE `detail_pesanan`
  ADD PRIMARY KEY (`id`),
  ADD KEY `pesanan_id` (`pesanan_id`),
  ADD KEY `kItem_id` (`kItem_id`);

--
-- Indexes for table `kategori_produk`
--
ALTER TABLE `kategori_produk`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `keranjang_item`
--
ALTER TABLE `keranjang_item`
  ADD PRIMARY KEY (`id`),
  ADD KEY `pengguna_id` (`pengguna_id`),
  ADD KEY `produk_id` (`produk_id`);

--
-- Indexes for table `metode_pembayaran`
--
ALTER TABLE `metode_pembayaran`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `pengguna`
--
ALTER TABLE `pengguna`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `pesanan`
--
ALTER TABLE `pesanan`
  ADD PRIMARY KEY (`id`),
  ADD KEY `metode_id` (`metode_id`);

--
-- Indexes for table `produk`
--
ALTER TABLE `produk`
  ADD PRIMARY KEY (`id`),
  ADD KEY `kategori_id` (`kategori_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `detail_pesanan`
--
ALTER TABLE `detail_pesanan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT for table `kategori_produk`
--
ALTER TABLE `kategori_produk`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `keranjang_item`
--
ALTER TABLE `keranjang_item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT for table `metode_pembayaran`
--
ALTER TABLE `metode_pembayaran`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `pengguna`
--
ALTER TABLE `pengguna`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=66;

--
-- AUTO_INCREMENT for table `pesanan`
--
ALTER TABLE `pesanan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `produk`
--
ALTER TABLE `produk`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=47;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `detail_pesanan`
--
ALTER TABLE `detail_pesanan`
  ADD CONSTRAINT `detail_pesanan_ibfk_1` FOREIGN KEY (`pesanan_id`) REFERENCES `pesanan` (`id`),
  ADD CONSTRAINT `detail_pesanan_ibfk_2` FOREIGN KEY (`kItem_id`) REFERENCES `keranjang_item` (`id`);

--
-- Constraints for table `keranjang_item`
--
ALTER TABLE `keranjang_item`
  ADD CONSTRAINT `keranjang_item_ibfk_1` FOREIGN KEY (`pengguna_id`) REFERENCES `pengguna` (`id`),
  ADD CONSTRAINT `keranjang_item_ibfk_2` FOREIGN KEY (`produk_id`) REFERENCES `produk` (`id`);

--
-- Constraints for table `pesanan`
--
ALTER TABLE `pesanan`
  ADD CONSTRAINT `pesanan_ibfk_1` FOREIGN KEY (`metode_id`) REFERENCES `metode_pembayaran` (`id`);

--
-- Constraints for table `produk`
--
ALTER TABLE `produk`
  ADD CONSTRAINT `produk_ibfk_1` FOREIGN KEY (`kategori_id`) REFERENCES `kategori_produk` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
