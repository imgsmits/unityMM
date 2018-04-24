-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Apr 12, 2018 at 09:06 AM
-- Server version: 5.7.19
-- PHP Version: 5.6.31

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `masterymind`
--

-- --------------------------------------------------------

--
-- Table structure for table `logentries`
--

DROP TABLE IF EXISTS `logentries`;
CREATE TABLE IF NOT EXISTS `logentries` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `timestamp` float NOT NULL,
  `userid` varchar(255) NOT NULL,
  `event` varchar(255) NOT NULL,
  `param1` int(11) NOT NULL,
  `param2` int(11) NOT NULL,
  `param3` int(11) NOT NULL,
  `param4` varchar(255) NOT NULL,
  `param5` float NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=372 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `logentries`
--

INSERT INTO `logentries` (`id`, `timestamp`, `userid`, `event`, `param1`, `param2`, `param3`, `param4`, `param5`) VALUES
(371, 0, 'xxx-user-xxx-6549', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(370, 0, 'xxx-user-xxx-6549', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(369, 0, 'xxx-user-xxx-6549', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(368, 0, 'xxx-user-xxx-6549', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(367, 0, 'xxx-user-xxx-6549', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(366, 0, 'xxx-user-xxx-6549', 'EVENT_GAME_START', 3, 4, 10, 'Code(111)', 0),
(365, 811.565, 'xxx-user-xxx-4079', 'EVENT_SESSION_END', 0, 0, 0, '', 0),
(364, 811.547, 'xxx-user-xxx-4079', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(363, 811.547, 'xxx-user-xxx-4079', 'EVENT_GAME_QUIT', 365, 0, 0, '', 0),
(362, 807.26, 'xxx-user-xxx-4079', 'EVENT_GAME_START', 3, 4, 10, 'Code(210)', 0),
(361, 807.26, 'xxx-user-xxx-4079', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(360, 807.241, 'xxx-user-xxx-4079', 'EVENT_DIFFICULTY_HIDE', 0, 0, 0, '', 4.86902),
(359, 807.241, 'xxx-user-xxx-4079', 'EVENT_DIFFICULTY_SET', 3, 4, 0, 'EASIER', 0),
(358, 802.372, 'xxx-user-xxx-4079', 'EVENT_DIFFICULTY_SHOW', 0, 0, 0, '', 0),
(357, 802.351, 'xxx-user-xxx-4079', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(356, 797.835, 'xxx-user-xxx-4079', 'EVENT_GAME_SKIPPED', 365, 0, 3, '', 0),
(355, 760.55, 'xxx-user-xxx-4079', 'EVENT_GAME_LB_HIDE', 365, 0, 10, '', 1.41846),
(354, 759.132, 'xxx-user-xxx-4079', 'EVENT_GAME_LB_SHOW', 365, 0, 10, '', 0),
(353, 756.716, 'xxx-user-xxx-4079', 'EVENT_GAME_LB_HIDE', 365, 0, 10, '', 180.993),
(352, 575.723, 'xxx-user-xxx-4079', 'EVENT_GAME_LB_SHOW', 365, 0, 10, '', 0),
(351, 573.789, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 0, 0, 3, 'Code(0000)', 0),
(350, 477.634, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 1, 1, 2, 'Code(0320)', 0),
(349, 469.118, 'xxx-user-xxx-4079', 'EVENT_GAME_HINT', 0, 0, 0, 'Code(0320)', 0),
(348, 458.6, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 1, 1, 1, 'Code(1301)', 0),
(347, 440.233, 'xxx-user-xxx-4079', 'EVENT_GAME_CHEAT', 1, 3, 0, '', 0),
(346, 414.313, 'xxx-user-xxx-4079', 'EVENT_GAME_START', 4, 5, 10, 'Code(3312)', 0),
(345, 414.313, 'xxx-user-xxx-4079', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(344, 414.296, 'xxx-user-xxx-4079', 'EVENT_DIFFICULTY_HIDE', 0, 0, 0, '', 76.7076),
(343, 414.296, 'xxx-user-xxx-4079', 'EVENT_DIFFICULTY_SET', 4, 5, 0, 'HARDER', 0),
(342, 337.588, 'xxx-user-xxx-4079', 'EVENT_DIFFICULTY_SHOW', 0, 0, 0, '', 0),
(341, 337.568, 'xxx-user-xxx-4079', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(340, 208.294, 'xxx-user-xxx-4079', 'EVENT_GAME_WON', 335, 0, 5, '', 0),
(339, 208.294, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 3, 0, 5, 'Code(121)', 0),
(338, 205.861, 'xxx-user-xxx-4079', 'EVENT_GAME_HINT', 0, 0, 0, 'Code(121)', 0),
(336, 174.443, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 1, 1, 3, 'Code(201)', 0),
(337, 186.945, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 1, 0, 4, 'Code(103)', 0),
(335, 149.725, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 2, 0, 2, 'Code(101)', 0),
(334, 141.208, 'xxx-user-xxx-4079', 'EVENT_GAME_MOVE', 1, 0, 1, 'Code(001)', 0),
(333, 103.233, 'xxx-user-xxx-4079', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(332, 103.233, 'xxx-user-xxx-4079', 'EVENT_GAME_START', 3, 4, 10, 'Code(121)', 0),
(331, 103.183, 'xxx-user-xxx-2110', 'EVENT_SESSION_USERNAME', 0, 0, 0, 'Eelcor', 0),
(329, 23.6561, 'xxx-user-xxx-2110', 'EVENT_SESSION_END', 0, 0, 0, '', 0),
(330, 0, 'xxx-user-xxx-2110', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(328, 23.6389, 'xxx-user-xxx-2110', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(327, 23.6389, 'xxx-user-xxx-2110', 'EVENT_GAME_QUIT', 410, 0, 0, '', 0),
(325, 22.3495, 'xxx-user-xxx-2110', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(326, 22.3495, 'xxx-user-xxx-2110', 'EVENT_GAME_START', 4, 5, 10, 'Code(0031)', 0),
(324, 22.3345, 'xxx-user-xxx-2110', 'EVENT_DIFFICULTY_SET', 4, 5, 0, 'HARDER', 0),
(322, 21.0947, 'xxx-user-xxx-2110', 'EVENT_DIFFICULTY_SHOW', 0, 0, 0, '', 0),
(323, 22.3345, 'xxx-user-xxx-2110', 'EVENT_DIFFICULTY_HIDE', 0, 0, 0, '', 1.2398),
(321, 21.0778, 'xxx-user-xxx-2110', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(320, 20.1429, 'xxx-user-xxx-2110', 'EVENT_GAME_WON', 410, 0, 3, '', 0),
(319, 20.1429, 'xxx-user-xxx-2110', 'EVENT_GAME_MOVE', 3, 0, 3, 'Code(201)', 0),
(318, 15.1515, 'xxx-user-xxx-2110', 'EVENT_GAME_MOVE', 2, 0, 2, 'Code(101)', 0),
(317, 12.3303, 'xxx-user-xxx-2110', 'EVENT_GAME_LB_HIDE', 10, 0, 10, '', 3.4054),
(316, 8.9249, 'xxx-user-xxx-2110', 'EVENT_GAME_LB_SHOW', 10, 0, 10, '', 0),
(315, 8.1901, 'xxx-user-xxx-2110', 'EVENT_GAME_MOVE', 1, 0, 1, 'Code(000)', 0),
(314, 5.001, 'xxx-user-xxx-2110', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(313, 4.8795, 'xxx-user-xxx-2110', 'EVENT_GAME_START', 3, 4, 10, 'Code(201)', 0),
(312, 4.8122, '', 'EVENT_SESSION_USERNAME', 0, 0, 0, 'Rose', 0),
(311, 0, '', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(310, 35.7253, 'xxx-user-xxx-6522', 'EVENT_SESSION_END', 0, 0, 0, '', 0),
(309, 35.7082, 'xxx-user-xxx-6522', 'EVENT_GAME_QUIT', 420, 0, 0, '', 0),
(308, 35.7082, 'xxx-user-xxx-6522', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(307, 30.8027, 'xxx-user-xxx-6522', 'EVENT_GAME_LB_HIDE', 420, 0, 10, '', 5.54977),
(306, 25.2529, 'xxx-user-xxx-6522', 'EVENT_GAME_LB_SHOW', 420, 0, 10, '', 0),
(305, 21.3099, 'xxx-user-xxx-6522', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(304, 21.3099, 'xxx-user-xxx-6522', 'EVENT_GAME_START', 3, 4, 10, 'Code(210)', 0),
(303, 21.293, 'xxx-user-xxx-6522', 'EVENT_DIFFICULTY_SET', 3, 4, 0, 'SAME', 0),
(302, 21.293, 'xxx-user-xxx-6522', 'EVENT_DIFFICULTY_HIDE', 0, 0, 0, '', 1.26683),
(301, 20.0262, 'xxx-user-xxx-6522', 'EVENT_DIFFICULTY_SHOW', 0, 0, 0, '', 0),
(300, 20.0093, 'xxx-user-xxx-6522', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(299, 18.813, 'xxx-user-xxx-6522', 'EVENT_GAME_WON', 420, 0, 3, '', 0),
(298, 18.813, 'xxx-user-xxx-6522', 'EVENT_GAME_MOVE', 3, 0, 3, 'Code(200)', 0),
(297, 12.6968, 'xxx-user-xxx-6522', 'EVENT_GAME_MOVE', 2, 0, 2, 'Code(100)', 0),
(296, 9.49557, 'xxx-user-xxx-6522', 'EVENT_GAME_MOVE', 2, 0, 1, 'Code(000)', 0),
(295, 5.22973, 'xxx-user-xxx-6522', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(294, 5.22973, 'xxx-user-xxx-6522', 'EVENT_GAME_START', 3, 4, 10, 'Code(200)', 0),
(293, 5.21363, 'xxx-user-xxx-1152', 'EVENT_SESSION_USERNAME', 0, 0, 0, 'Napoleon', 0),
(292, 0, 'xxx-user-xxx-1152', 'EVENT_SESSION_START', 0, 0, 0, '', 0),
(291, 5.00205, 'xxx-user-xxx-1152', 'EVENT_GAME_QUIT_AVAILABLE', 0, 0, 0, '', 0),
(290, 0, 'xxx-user-xxx-1152', 'EVENT_GAME_START', 3, 4, 10, 'Code(121)', 0),
(289, 13.7392, 'xxx-user-xxx-5268', 'EVENT_GAME_LB_HIDE', 20, 0, 10, '', 0.977236),
(288, 12.7619, 'xxx-user-xxx-5268', 'EVENT_GAME_LB_SHOW', 20, 0, 10, '', 0),
(287, 11.4445, 'xxx-user-xxx-5268', 'EVENT_GAME_START', 3, 4, 10, 'Code(001)', 0),
(286, 11.4275, 'xxx-user-xxx-5268', 'EVENT_DIFFICULTY_HIDE', 0, 0, 0, '', 0.879307),
(285, 11.4275, 'xxx-user-xxx-5268', 'EVENT_DIFFICULTY_SET', 3, 4, 0, 'SAME', 0),
(284, 10.5482, 'xxx-user-xxx-5268', 'EVENT_DIFFICULTY_SHOW', 0, 0, 0, '', 0),
(283, 10.5311, 'xxx-user-xxx-5268', 'EVENT_GAME_END', 0, 0, 0, '', 0),
(282, 9.72239, 'xxx-user-xxx-5268', 'EVENT_GAME_SKIPPED', 20, 0, 2, '', 0),
(281, 8.39156, 'xxx-user-xxx-5268', 'EVENT_GAME_MOVE', 1, 0, 2, 'Code(000)', 0),
(280, 5.99873, 'xxx-user-xxx-5268', 'EVENT_GAME_MOVE', 1, 0, 1, 'Code(000)', 0),
(279, 2.8284, 'xxx-user-xxx-5268', 'EVENT_GAME_START', 3, 4, 10, 'Code(022)', 0),
(278, 2.81221, 'xxx-user-xxx-2578', 'EVENT_SESSION_USERNAME', 0, 0, 0, 'Pierre', 0),
(277, 0, 'xxx-user-xxx-2578', 'EVENT_SESSION_START', 0, 0, 0, '', 0);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
