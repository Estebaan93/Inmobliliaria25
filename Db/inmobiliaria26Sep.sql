-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-09-2025 a las 21:58:15
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--
CREATE DATABASE IF NOT EXISTS `inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_spanish_ci;
USE `inmobiliaria`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoria`
--

CREATE TABLE `auditoria` (
  `idAuditoria` int(11) NOT NULL,
  `idEntidad` int(11) NOT NULL,
  `tipoEntidad` enum('contrato','pago') NOT NULL,
  `accion` enum('crear','terminar','anular') NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `observacion` text DEFAULT NULL,
  `fechaYHora` datetime NOT NULL DEFAULT current_timestamp(),
  `estado` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `auditoria`
--

INSERT INTO `auditoria` (`idAuditoria`, `idEntidad`, `tipoEntidad`, `accion`, `idUsuario`, `observacion`, `fechaYHora`, `estado`) VALUES
(1, 20, 'contrato', 'crear', 1, 'Contrato creado (N° 20)', '2025-09-23 11:37:08', 1),
(2, 21, 'contrato', 'crear', 6, 'Contrato creado (N° 21)', '2025-09-23 11:38:48', 1),
(3, 21, 'contrato', 'anular', 1, 'Contrato anulado (N° 21)', '2025-09-23 11:39:29', 1),
(4, 0, 'pago', 'crear', 1, 'Multa creada por anulación de contrato (Contrato 21, Pago N° 0)', '2025-09-23 11:39:29', 1),
(5, 17, 'contrato', 'anular', 1, 'Contrato anulado (N° 17)', '2025-09-24 08:09:46', 1),
(6, 0, 'pago', 'crear', 1, 'Multa creada por anulación de contrato (Contrato 17, Pago N° 0)', '2025-09-24 08:09:46', 1),
(7, 0, 'pago', 'crear', 1, 'Pago registrado (Contrato 16, N° 2)', '2025-09-24 11:03:55', 1),
(8, 22, 'contrato', 'crear', 1, 'Contrato creado (N° 22)', '2025-09-24 11:07:09', 1),
(10, 0, 'pago', 'crear', 5, 'Pago registrado (Contrato N° 18, N° 1)', '2025-09-25 22:30:36', 1),
(11, 0, 'pago', 'crear', 5, 'Pago registrado (Contrato N° 22, N° 1)', '2025-09-25 22:45:00', 1),
(12, 0, 'pago', 'crear', 5, 'Pago registrado (Contrato N° 22, pago N° 2)', '2025-09-25 22:54:27', 1),
(13, 33, 'pago', 'anular', 5, 'Pago eliminado (N° 33)', '2025-09-25 23:08:51', 1),
(14, 0, 'pago', 'crear', 5, 'Pago registrado (Contrato N° 15, pago N° 2)', '2025-09-25 23:09:03', 1),
(15, 0, 'pago', 'crear', 5, 'Pago registrado (Contrato N° 16, pago N° 3)', '2025-09-26 00:03:44', 1),
(16, 20, 'contrato', 'anular', 5, 'Contrato anulado (N° 20)', '2025-09-26 00:04:21', 1),
(17, 0, 'pago', 'crear', 5, 'Multa creada por anulación de contrato (Contrato N° 20, Pago N° 0)', '2025-09-26 00:04:21', 1),
(18, 19, 'contrato', 'anular', 5, 'Contrato anulado (N° 19)', '2025-09-26 00:09:21', 1),
(19, 0, 'pago', 'crear', 5, 'Multa creada por anulación de contrato (Contrato N° 19, Pago N° 0)', '2025-09-26 00:09:21', 1),
(20, 16, 'contrato', 'anular', 5, 'Contrato anulado (N° 16)', '2025-09-26 00:21:37', 1),
(21, 45, 'pago', 'crear', 5, 'Multa creada por anulación de contrato (Contrato N° 16, Pago N° 45)', '2025-09-26 00:21:37', 1),
(22, 18, 'contrato', 'anular', 5, 'Contrato anulado (N° 18)', '2025-09-26 00:23:12', 1),
(23, 46, 'pago', 'crear', 5, 'Multa creada por anulación de contrato (Contrato N° 18, Pago N° 46)', '2025-09-26 00:23:12', 1),
(24, 22, 'contrato', 'anular', 5, 'Contrato anulado (N° 22)', '2025-09-26 00:24:15', 1),
(25, 47, 'pago', 'crear', 5, 'Multa creada por anulación de contrato (Contrato N° 22, Pago N° 47)', '2025-09-26 00:24:15', 1),
(26, 23, 'contrato', 'crear', 5, 'Contrato creado (N° 23)', '2025-09-26 00:39:42', 1),
(27, 0, 'pago', 'crear', 1, 'Pago registrado (Contrato N° 14, pago N° 2)', '2025-09-26 01:29:07', 1),
(28, 48, 'pago', 'anular', 1, 'Pago eliminado (N° 48)', '2025-09-26 01:31:10', 1),
(29, 14, 'contrato', 'anular', 1, 'Contrato anulado (N° 14)', '2025-09-26 01:31:20', 1),
(30, 49, 'pago', 'crear', 1, 'Multa creada por anulación de contrato (Contrato N° 14, Pago N° 49)', '2025-09-26 01:31:20', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `idContrato` int(11) NOT NULL,
  `idInquilino` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `monto` decimal(10,0) NOT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `FechaAnulacion` datetime DEFAULT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`idContrato`, `idInquilino`, `idInmueble`, `monto`, `fechaInicio`, `fechaFin`, `FechaAnulacion`, `estado`) VALUES
(14, 1, 5, 1300000, '2025-09-26', '2026-04-27', '2025-09-26 00:00:00', 0),
(15, 1, 7, 150000, '2025-09-21', '2025-12-23', '2025-09-21 00:00:00', 0),
(16, 5, 6, 700000, '2025-09-21', '2025-12-25', '2025-09-26 00:00:00', 0),
(17, 1, 5, 250000, '2025-09-21', '2025-10-29', '2025-09-24 00:00:00', 0),
(18, 1, 2, 600000, '2025-09-22', '2025-10-22', '2025-09-26 00:00:00', 0),
(19, 8, 7, 150000, '2025-09-22', '2025-11-22', '2025-09-26 00:00:00', 0),
(20, 1, 1, 480000, '2025-09-23', '2025-11-23', '2025-09-26 00:00:00', 0),
(21, 7, 4, 200000, '2025-09-23', '2025-10-29', '2025-09-23 00:00:00', 0),
(22, 1, 4, 200000, '2025-09-24', '2025-11-24', '2025-09-26 00:00:00', 0),
(23, 1, 8, 1200000, '2025-09-24', '2025-09-25', NULL, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `direccion`
--

CREATE TABLE `direccion` (
  `idDireccion` int(11) NOT NULL,
  `calle` varchar(200) NOT NULL,
  `altura` int(11) NOT NULL,
  `cp` varchar(12) NOT NULL,
  `ciudad` varchar(100) NOT NULL,
  `coordenadas` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `direccion`
--

INSERT INTO `direccion` (`idDireccion`, `calle`, `altura`, `cp`, `ciudad`, `coordenadas`) VALUES
(1, 'Pasaje San Vicente', 266, '5700', 'San Luis', '-33.28945885220947, -66.3199532064231'),
(2, 'Don bosco y Centenario', 400, '5700', 'San Luis', '-33.27974591009094, -66.3330368080857'),
(3, 'Aristobulo Del Valle ', 202, '5700', 'San Luis', '-33.28678657693836, -66.32073087563543'),
(4, 'Av. circuito A3', 1000, '5702', 'Potrero de los Funes San Luis', '-33.21934904785864, -66.2305082738294'),
(5, 'Soldado Puntano Desconocido', 485, '5700', 'San Luis', '-33.29377803214514, -66.31734272147676'),
(6, 'Lavalle', 542, '5700', 'San Luis', '-33.29849595664121, -66.33191817510935'),
(7, 'San Martin', 848, '5700', '', '-33.30087731332195, -66.33783526465731'),
(8, 'Lavalle', 301, '5700', 'San Luis', '-33.29830645577751, -66.3318880889465'),
(10, 'Nologi', 100, '5730', 'San Luis', '-32.922691711111526, -66.32630119109417'),
(11, 'Nologi', 100, '5730', 'San Luis', '-32.922691711111526, -66.32630119109417'),
(12, 'Nologi', 100, '5730', 'San Luis', '-32.922691711111526, -66.32630119109417'),
(13, 'San Francisco', 200, '5731', 'San Luis', '-32.606266946900746, -66.12285166613296');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `idInmueble` int(11) NOT NULL,
  `idPropietario` int(11) NOT NULL,
  `idDireccion` int(11) NOT NULL,
  `idTipo` int(11) NOT NULL,
  `metros2` varchar(100) NOT NULL,
  `cantidadAmbientes` int(11) NOT NULL,
  `precio` decimal(10,0) NOT NULL,
  `descripcion` varchar(300) NOT NULL,
  `cochera` tinyint(4) NOT NULL,
  `piscina` tinyint(4) NOT NULL,
  `mascotas` tinyint(4) NOT NULL,
  `urlImagen` varchar(400) NOT NULL,
  `estado` tinyint(2) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`idInmueble`, `idPropietario`, `idDireccion`, `idTipo`, `metros2`, `cantidadAmbientes`, `precio`, `descripcion`, `cochera`, `piscina`, `mascotas`, `urlImagen`, `estado`) VALUES
(1, 1, 1, 1, 'Son cien metros cuadrados', 2, 480000, 'Lindo lugar', 0, 0, 0, 'https://conceptotallerdearquitectura.com/wp-content/uploads/2022/11/36.jpg', 1),
(2, 1, 2, 2, '60', 2, 600000, 'Local amplio y buena ubicacion', 1, 0, 0, 'https://areazero20.com/wp-content/uploads/2022/04/diseno-de-locales-comerciales-1024x683.jpg', 0),
(3, 3, 3, 2, '35', 1, 300000, 'Centro de estetica', 1, 0, 1, 'https://www.vulka.es/imagenes/empresas_fotos/314389_big.jpg', 0),
(4, 2, 4, 1, '50', 2, 200000, 'luminoiso', 1, 0, 0, 'https://www.cabanias.com.ar/fotos/pinar-del-valle.022.b1.jpg', 0),
(5, 6, 5, 3, '25', 3, 250000, 'Buena ubicacion, zona centro', 1, 1, 1, 'https://imgar.zonapropcdn.com/avisos/resize/1/00/57/13/66/98/1200x1200/2000194540.jpg?isFirstImage=true', 0),
(6, 3, 6, 3, '40', 3, 700000, '2 pisos muy comodo', 1, 0, 1, 'https://www.construyehogar.com/wp-content/uploads/2014/07/Dise%C3%B1o-de-casa-sencilla-de-dos-pisos.jpg', 0),
(7, 9, 7, 4, '90', 2, 150000, 'Zona centro, departamentos en torre Regidor', 1, 0, 0, 'https://static.wixstatic.com/media/85b2a7_df88f35529af413b8052c43aacfa564a.jpg/v1/fill/w_815,h_462,al_c,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/85b2a7_df88f35529af413b8052c43aacfa564a.jpg', 0),
(8, 1, 8, 1, '60', 2, 1200000, 'A estrenar', 1, 0, 1, 'https://atipik.com.ar/wp-content/uploads/2018/09/42059220_2056113404399694_2312482066580635648_n.jpg', 1),
(12, 1, 12, 5, '40', 5, 400000, 'Naturaleza', 1, 1, 1, 'https://www.novaparks.com/sites/default/files/styles/scale_1024/public/2025-03/PohickBayPark20240831-NP-03%20-%20web.jpg?itok=YwR0apcl', 1),
(13, 3, 13, 5, '40', 0, 400000, 'Hermoso lugar', 1, 1, 1, 'https://images.pexels.com/photos/1687845/pexels-photo-1687845.jpeg?_gl=1*2txwf5*_ga*MTYzNjEzODgwLjE3NTg5MTU5ODE.*_ga_8JE65Q40S6*czE3NTg5MTU5ODAkbzEkZzEkdDE3NTg5MTYwMDAkajQwJGwwJGgw', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `idInquilino` int(11) NOT NULL,
  `dni` varchar(10) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `telefono` varchar(60) NOT NULL,
  `correo` varchar(150) NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`idInquilino`, `dni`, `apellido`, `nombre`, `telefono`, `correo`, `estado`) VALUES
(1, '37599292', 'Moreira', 'Esteban', '0114008713', 'esteban@correo.com', 1),
(2, '31000123', 'Vasquez', 'Nicolas', '01133191239', 'nico@correo.com', 1),
(3, '39550113', 'Nuñez', 'Facundo', '2664087634', 'facu@correo.com', 1),
(4, '301123231', 'Mercau', 'Gladys', '2664001323 esteban', 'mercau@correo.com; gladys@correo.com', 1),
(5, '30123456', 'Gómez', 'María', '2664123456', 'maria.gomez@mail.com', 0),
(6, '29222333', 'Pérez', 'Juan', '2664987654', 'juan.perez@mail.com', 1),
(7, '30333444', 'López', 'Ana', '2664765432', 'ana.lopez@mail.com', 0),
(8, '31555666', 'Fernández', 'Carlos', '2664556677', 'carlos.fernandez@mail.com', 0),
(9, '28888999', 'Rodríguez', 'Lucía', '2664332211', 'lucia.rodriguez@mail.com', 1),
(10, '32233444', 'Sosa', 'Martín', '2664667788', 'martin.sosa@mail.com', 1),
(11, '27777888', 'Ramírez', 'Sofía Laura', '2664001122', 'sofia.ramirez@mail.com', 1),
(12, '30011223', 'Torres', 'Diego', '2664223344', 'diego.torres@mail.com', 1),
(13, '31122334', 'Castro', 'Paula', '2664558899', 'paula.castro@mail.com', 0),
(14, '29999888', 'Moreno', 'Javier', '2664776655', 'javier.moreno@mail.com', 1),
(15, '28777666', 'Romero', 'Carla', '2664227766', 'carla.romero@mail.com', 1),
(16, '31222111', 'Vega', 'Esteban', '2664335544', 'esteban.vega@mail.com', 0),
(17, '30999111', 'Molina', 'Valeria', '2664993322', 'valeria.molina@mail.com', 1),
(18, '29777123', 'Navarro', 'Fernando', '2664111222', 'fernando.navarro@mail.com', 1),
(19, '28999123', 'Suárez', 'Julieta', '2664445566', 'julieta.suarez@mail.com', 1),
(20, '46001233', 'Rodriguez', 'James', '011987123', 'james@correo.com', 1),
(21, '60333123', 'Escudero', 'Samuel', '232000434', 'samu@correo.com', 0),
(22, '12345678', 'Pruebas', 'Pruebas', '2444345345', 'pruebas@correo.com', 0),
(23, '123456789', 'Pruebas de apellido', 'Pruebas de nombre', '2444345345', 'pruebas2@correo.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `idPago` int(11) NOT NULL,
  `idContrato` int(11) NOT NULL,
  `fechaPago` date NOT NULL,
  `importe` decimal(10,0) NOT NULL,
  `numeroPago` varchar(50) NOT NULL,
  `detalle` varchar(200) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`idPago`, `idContrato`, `fechaPago`, `importe`, `numeroPago`, `detalle`, `estado`) VALUES
(30, 14, '2025-09-20', 3900000, 'Multa', 'Multa por contrato anulado (incluye deuda: 1300000)', 0),
(31, 14, '2025-09-20', 3900000, '1', 'Multa por contrato anulado (incluye deuda: 1300000)', 0),
(32, 15, '2025-09-21', 450000, 'Multa', 'Multa por contrato anulado (incluye deuda: 150000)', 0),
(33, 15, '2025-09-21', 450000, '1', 'Multa por contrato anulado (incluye deuda: 150000)', 0),
(34, 16, '2025-09-21', 700000, '1', 'Abonado', 1),
(35, 21, '2025-09-23', 600000, 'Multa', 'Multa por contrato anulado (incluye deuda: 200000)', 0),
(36, 17, '2025-09-24', 750000, 'Multa', 'Multa por contrato anulado (incluye deuda: 250000)', 0),
(37, 16, '2025-09-24', 700000, '2', 'Mes adelantado de octubre', 1),
(38, 18, '2025-09-25', 600000, '1', 'Mes septiembre ', 1),
(39, 22, '2025-09-25', 200000, '1', 'agosto atrasado', 1),
(40, 22, '2025-09-25', 200000, '2', 'Pago de septiembre', 1),
(41, 15, '2025-09-25', 450000, '2', 'Multa por contrato anulado (incluye deuda: 150000)', 1),
(42, 16, '2025-09-26', 700000, '3', 'Mes de noviembre', 1),
(43, 20, '2025-09-26', 1440000, 'Multa', 'Multa por contrato anulado (incluye deuda: 480000)', 0),
(44, 19, '2025-09-26', 450000, 'Multa', 'Multa por contrato anulado (incluye deuda: 150000)', 0),
(45, 16, '2025-09-26', 1400000, 'Multa', 'Multa por contrato anulado (incluye deuda: 0)', 1),
(46, 18, '2025-09-26', 1200000, 'Multa', 'Multa por contrato anulado (incluye deuda: 0)', 1),
(47, 22, '2025-09-26', 400000, 'Multa', 'Multa por contrato anulado (incluye deuda: 0)', 1),
(48, 14, '2025-09-26', 1300000, '2', 'Mes de septiembre', 0),
(49, 14, '2025-09-26', 3900000, 'Multa', 'Multa por contrato anulado (incluye deuda: 1300000)', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `idPropietario` int(11) NOT NULL,
  `dni` varchar(10) NOT NULL,
  `apellido` varchar(60) NOT NULL,
  `nombre` varchar(60) NOT NULL,
  `telefono` varchar(15) NOT NULL,
  `correo` varchar(100) NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`idPropietario`, `dni`, `apellido`, `nombre`, `telefono`, `correo`, `estado`) VALUES
(1, '37599292', 'Moreira', 'Esteban', '2665044026', 'esteban@correo.com', 1),
(2, '32001031', 'Iccardi', 'Gimena', '011336648', 'gime@correo.com', 1),
(3, '39599145', 'Rosales', 'Agustina', '2665008861', 'agus@correo.com', 1),
(4, '30111222', 'Pérez', 'Carlos', '2664558899', 'carlos.perez@mail.com', 1),
(5, '32233444', 'Gómez', 'María', '2664771122', 'maria.gomez@mail.com', 1),
(6, '28999888', 'López', 'Juan', '2664332211', 'juan.lopez@mail.com', 1),
(7, '31122333', 'Fernández', 'Lucia', '2664667788', 'lucia.fernandez@mail.com', 1),
(8, '30555111', 'Ramírez', 'Diego', '2664001234', 'diego.ramirez@mail.com', 1),
(9, '2037599292', 'GRUPO', 'ASIS', '0119873123', 'ASIS@correo.com', 1),
(10, '46001231', 'Rodriguez', 'James', '0115554123', 'james1@correo.com', 1),
(11, '46001233', 'Rodriguez2', 'James2', '0115554123', 'james2@correo.com', 1),
(12, '11111111', 'aaaa', 'aaaaaa', '123456789', 'aa@correo.com', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo`
--

CREATE TABLE `tipo` (
  `idTipo` int(11) NOT NULL,
  `observacion` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo`
--

INSERT INTO `tipo` (`idTipo`, `observacion`) VALUES
(5, 'Camping'),
(3, 'Casa'),
(1, 'Departamento'),
(2, 'Local'),
(4, 'Torre');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `rol` varchar(50) NOT NULL,
  `avatar` varchar(150) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`idUsuario`, `email`, `password`, `rol`, `avatar`, `nombre`, `apellido`, `estado`) VALUES
(1, 'esteban@correo.com', '$2a$12$TwyfaC03f0e8CB2ij1BgjuzqAvIDphV/2coBwrYCWI/PEsv780g12', 'Administrador', '/img/avatars/Esteban_Moreira.jpg', 'Esteban ', 'Moreira', 1),
(2, 'samanta@correo.com', '$2a$12$.Usstiq1CfslMzLGv8/meeN/bnUtuUxqeqRtpAIEopniCIaYbCnh.', 'Empleado', '/img/avatars/default.jpg', 'Samanta', 'Zabaletta', 0),
(5, 'maricel@correo.com', '$2a$12$I8YwScH9BLY0B1EGDXHNLO4StWm.7UyAvT0i1n3555X.6asNdG1xi', 'Empleado', '/img/avatars/default.jpg', 'Yamila Maricel', 'Luzza', 1),
(6, 'mary@correo.com', '$2a$12$NIjF/kP6QroVK8pAzBKIKuB68o5YJoTKqm5/j3r2IrIldZZOT360m', 'Empleado', '/img/avatars/dualipa.jpeg', 'Marilena Soledad', 'Escudero', 1),
(7, 'olga@gmail.com', '$2a$12$aPsVy0y9UWr/e.p7tgTzJu01/FNGea/Z1iR2EvUzWxQ/wP97QuZtW', 'Administrador', '/img/avatars/default.jpg', 'Olgita', 'Lucero', 0),
(8, 'yaniR@example.com', '$2a$12$.aRxuCDu0jIQStJSP8WP1.tY7GBzPPn366UstY0o.qNd602.08io2', 'Administrador', '/img/avatars/default.jpg', 'Yanina', 'Rosales', 0),
(9, 'dua@correo.com', '$2a$12$nSMxnBJor.5dYM9T2oSuuuLVq8zryYk8WUfk5AgOdwMgDW8/FlId.', 'Empleado', '/img/avatars/dualipa.jpeg', 'Dua', 'Lipa', 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD PRIMARY KEY (`idAuditoria`),
  ADD KEY `fk_auditoria_usuario` (`idUsuario`),
  ADD KEY `idx_entidad` (`tipoEntidad`,`idEntidad`);

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`idContrato`),
  ADD KEY `idInquilino` (`idInquilino`,`idInmueble`),
  ADD KEY `idInmueble` (`idInmueble`);

--
-- Indices de la tabla `direccion`
--
ALTER TABLE `direccion`
  ADD PRIMARY KEY (`idDireccion`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`idInmueble`),
  ADD KEY `idDireccion` (`idDireccion`,`idTipo`),
  ADD KEY `idPropietario` (`idPropietario`),
  ADD KEY `idTipo` (`idTipo`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`idInquilino`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`idPago`),
  ADD KEY `idContrato` (`idContrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`idPropietario`);

--
-- Indices de la tabla `tipo`
--
ALTER TABLE `tipo`
  ADD PRIMARY KEY (`idTipo`),
  ADD UNIQUE KEY `observacion` (`observacion`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`idUsuario`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  MODIFY `idAuditoria` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `idContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT de la tabla `direccion`
--
ALTER TABLE `direccion`
  MODIFY `idDireccion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `idInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `idInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `idPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=50;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `idPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `tipo`
--
ALTER TABLE `tipo`
  MODIFY `idTipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD CONSTRAINT `fk_auditoria_usuario` FOREIGN KEY (`idUsuario`) REFERENCES `usuario` (`idUsuario`);

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`idInquilino`) REFERENCES `inquilino` (`idInquilino`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`idInmueble`) REFERENCES `inmueble` (`idInmueble`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`idTipo`) REFERENCES `tipo` (`idTipo`),
  ADD CONSTRAINT `inmueble_ibfk_2` FOREIGN KEY (`idDireccion`) REFERENCES `direccion` (`idDireccion`),
  ADD CONSTRAINT `inmueble_ibfk_3` FOREIGN KEY (`idPropietario`) REFERENCES `propietario` (`idPropietario`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`idContrato`) REFERENCES `contrato` (`idContrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
