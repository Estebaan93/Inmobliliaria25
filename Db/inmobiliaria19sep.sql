-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 20-09-2025 a las 00:23:09
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
CREATE DATABASE IF NOT EXISTS `inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_spanish2_ci;
USE `inmobiliaria`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoria`
--

CREATE TABLE `auditoria` (
  `idAuditoria` int(11) NOT NULL,
  `idEntidad` int(11) NOT NULL,
  `tipoEntidad` varchar(50) NOT NULL,
  `accion` varchar(50) NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `observacion` text DEFAULT NULL,
  `fechaYHora` datetime NOT NULL DEFAULT current_timestamp(),
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `auditoria`
--

INSERT INTO `auditoria` (`idAuditoria`, `idEntidad`, `tipoEntidad`, `accion`, `idUsuario`, `observacion`, `fechaYHora`, `estado`) VALUES
(4, 18, 'inquilino', 'crear', 1, 'Inquilino creado', '2025-09-19 18:39:58', 1),
(5, 19, 'inquilino', 'crear', 3, 'Inquilino creado', '2025-09-19 18:49:14', 1);

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
  `fechaAnulacion` date DEFAULT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`idContrato`, `idInquilino`, `idInmueble`, `monto`, `fechaInicio`, `fechaFin`, `fechaAnulacion`, `estado`) VALUES
(1, 2, 1, 850000, '2025-05-06', '2025-07-23', '2025-09-19', 0),
(2, 3, 1, 90000, '2025-09-19', '2025-09-22', NULL, 1);

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
  `coordenadas` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `direccion`
--

INSERT INTO `direccion` (`idDireccion`, `calle`, `altura`, `cp`, `ciudad`, `coordenadas`) VALUES
(1, 'Av Centenario', 250, '5700', 'San Luis', '-33.279663691275374, -66.33255982277237');

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
  `urlImagen` varchar(150) NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`idInmueble`, `idPropietario`, `idDireccion`, `idTipo`, `metros2`, `cantidadAmbientes`, `precio`, `descripcion`, `cochera`, `piscina`, `mascotas`, `urlImagen`, `estado`) VALUES
(1, 1, 1, 2, '30', 3, 90000, 'Linda ubicacion', 1, 0, 1, 'https://static1.sosiva451.com/63866521/d6955005-3732-4276-850e-30c249733f50_u_medium.jpg', 1);

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
(1, '12345561', 'Moreira', 'Esteban', '02665044026', 'esteban25@correo.com', 1),
(2, '40112331', 'Flores', 'Jorge', '02665008861', 'flores@correo.com', 1),
(3, '33012208', 'Rivero', 'Jonatan', '01156231245', 'rivero@correo.com', 1),
(4, '30123456', 'Gómez', 'Juan', '2234567890', 'juan.gomez@mail.com', 1),
(5, '28987654', 'Martínez', 'Ana', '2231234567', 'ana.martinez@mail.com', 1),
(6, '31234567', 'Pérez', 'Carlos', '2237654321', 'carlos.perez@mail.com', 1),
(7, '29876543', 'Rodríguez', 'Lucía', '2236789012', 'lucia.rodriguez@mail.com', 1),
(8, '32567890', 'Fernández', 'Martín', '2239876543', 'martin.fernandez@mail.com', 1),
(9, '30543210', 'López', 'Sofía', '2234561234', 'sofia.lopez@mail.com', 1),
(10, '31567890', 'Sánchez', 'Diego', '2233214567', 'diego.sanchez@mail.com', 1),
(11, '32765432', 'Romero', 'Valentina', '2236547890', 'valentina.romero@mail.com', 1),
(12, '29987654', 'Torres', 'Nicolás', '2237894561', 'nicolas.torres@mail.com', 1),
(13, '31098765', 'Ruiz', 'Camila', '2238901234', 'camila.ruiz@mail.com', 1),
(14, '55002301', 'Pedreira', 'Paz', '02664087634', 'paz@correo.com', 1),
(18, '11232145', 'Lipa', 'Dua', '06666559788', 'dua@correo.com', 1),
(19, '777889979', 'Nazario', 'Roberto', '445612365', 'roberto@correo.com', 1),
(20, '111111112', 'Jamaica', 'Rusa', '444444445', 'rusa@correo.com', 1);

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
(1, 1, '2025-09-19', 321000, '1', 'ingreso mes adelantado ', 0),
(2, 1, '2025-09-19', 5100000, 'Multa', 'Multa por contrato anulado (incluye deuda: 4250000)', 0);

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
(1, '11111111', 'Gomez', 'Sebastian', '012345699', 'seba@correo.com', 1),
(2, '37599292', 'Moreira', 'Esteban Nicolas', '0266155053941', 'esteban@correo.com', 1),
(3, '20123456', 'Alvarez', 'María', '2231112233', 'maria.alvarez@mail.com', 1),
(4, '22987654', 'González', 'Roberto', '2232223344', 'roberto.gonzalez@mail.com', 1),
(5, '21567890', 'López', 'Carla', '2233334455', 'carla.lopez@mail.com', 1),
(6, '23456789', 'Martínez', 'Jorge', '2234445566', 'jorge.martinez@mail.com', 1),
(7, '24567891', 'Fernández', 'Laura', '2235556677', 'laura.fernandez@mail.com', 1),
(8, '25678901', 'Pérez', 'Andrés', '2236667788', 'andres.perez@mail.com', 1),
(9, '26789012', 'Ramírez', 'Paula', '2237778899', 'paula.ramirez@mail.com', 0),
(10, '27890123', 'Torres', 'Ricardo', '2238889900', 'ricardo.torres@mail.com', 1),
(11, '28901234', 'Díaz', 'Silvia', '2239990011', 'silvia.diaz@mail.com', 0),
(12, '29012345', 'Ruiz', 'Fernando', '2231011121', 'fernando.ruiz@mail.com', 1),
(13, '53012012', 'Pedreira', 'Paz', '02664087634', 'paz@correo.com', 1),
(14, '201456327', 'TORRE', 'MDQ', '5564788852', 'mdq@correo.com', 1),
(15, '77888777', 'aaaaaa', 'aaaaaa', '111111111', 'aaaaa@correo.com', 1);

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
(2, 'Casa'),
(1, 'Departamentos'),
(3, 'Local');

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
(1, 'esteban@correo.com', '$2a$12$boHxqTfP9oFp6Gc21i326eROerhHc1uCGkNmgbBmhjTrgCtalVblm', 'Administrador', '/img/avatars/esteban_moreira.jpg', 'Esteban', 'Moreira', 1),
(3, 'joa@correo.com', '$2a$12$J4bWNS1JKt0AEWdgJDWth.TDC/BQvqL3mhz2ibYIURbFGvJ96.7BO', 'Empleado', '/img/avatars/joa gomez.jpg', 'Joanas', 'Gomez', 1),
(4, 'estebanm_m@outlook.com', '$2a$12$lM.Tvsripvtvo2sVXcr9iOd.eKg45f5wPVilK1jrKvCHyng2U1v4u', 'Administrador', '/img/avatars/esteban_moreira.jpg', 'Esteban Nicolas', 'Moreira', 1);

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
  MODIFY `idAuditoria` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `idContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `direccion`
--
ALTER TABLE `direccion`
  MODIFY `idDireccion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `idInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `idInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `idPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `idPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `tipo`
--
ALTER TABLE `tipo`
  MODIFY `idTipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD CONSTRAINT `fk_auditoria_usuario` FOREIGN KEY (`idUsuario`) REFERENCES `usuario` (`idUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION;

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
