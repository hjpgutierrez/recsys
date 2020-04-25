/*
SQLyog Ultimate v11.11 (64 bit)
MySQL - 5.5.5-10.4.11-MariaDB : Database - recsys
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`recsys` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_spanish2_ci */;

USE `recsys`;

/*Table structure for table `comentario` */

DROP TABLE IF EXISTS `comentario`;

CREATE TABLE `comentario` (
  `IdComentario` int(11) NOT NULL AUTO_INCREMENT,
  `Observacion` text COLLATE utf8_spanish2_ci NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `IdReceta` int(11) NOT NULL,
  PRIMARY KEY (`IdComentario`),
  KEY `FK_ComentarioUsuario` (`IdUsuario`),
  KEY `FK_ComentarioReceta` (`IdReceta`),
  CONSTRAINT `FK_ComentarioReceta` FOREIGN KEY (`IdReceta`) REFERENCES `receta` (`IdReceta`),
  CONSTRAINT `FK_ComentarioUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

/*Data for the table `comentario` */

/*Table structure for table `detallereceta` */

DROP TABLE IF EXISTS `detallereceta`;

CREATE TABLE `detallereceta` (
  `IdDetalleReceta` int(11) NOT NULL AUTO_INCREMENT,
  `IdReceta` int(11) NOT NULL,
  `NombreIngrediente` varchar(15) COLLATE utf8_spanish2_ci NOT NULL,
  `Marca` varchar(15) COLLATE utf8_spanish2_ci NOT NULL,
  `CantidadIngrediente` double NOT NULL,
  `Medida` varchar(20) COLLATE utf8_spanish2_ci NOT NULL,
  `ValorIngrediente` double NOT NULL,
  `Subtotal` double NOT NULL,
  `Direccion` varchar(100) COLLATE utf8_spanish2_ci NOT NULL,
  PRIMARY KEY (`IdDetalleReceta`),
  KEY `FK_DetalleRecetaReceta` (`IdReceta`),
  CONSTRAINT `FK_DetalleRecetaReceta` FOREIGN KEY (`IdReceta`) REFERENCES `receta` (`IdReceta`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

/*Data for the table `detallereceta` */

/*Table structure for table `like` */

DROP TABLE IF EXISTS `like`;

CREATE TABLE `like` (
  `IdLike` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdUsuario` int(11) NOT NULL,
  `IdReceta` int(11) NOT NULL,
  PRIMARY KEY (`IdLike`),
  KEY `FK_LikeUsuario` (`IdUsuario`),
  KEY `FK_LikeReceta` (`IdReceta`),
  CONSTRAINT `FK_LikeReceta` FOREIGN KEY (`IdReceta`) REFERENCES `receta` (`IdReceta`),
  CONSTRAINT `FK_LikeUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

/*Data for the table `like` */

/*Table structure for table `perfil` */

DROP TABLE IF EXISTS `perfil`;

CREATE TABLE `perfil` (
  `IdPerfil` int(11) NOT NULL AUTO_INCREMENT,
  `NombrePerfil` varchar(100) COLLATE utf8_spanish2_ci NOT NULL,
  PRIMARY KEY (`IdPerfil`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

/*Data for the table `perfil` */

insert  into `perfil`(`IdPerfil`,`NombrePerfil`) values (1,'Administrador'),(2,'Usuario');

/*Table structure for table `receta` */

DROP TABLE IF EXISTS `receta`;

CREATE TABLE `receta` (
  `IdReceta` int(11) NOT NULL AUTO_INCREMENT,
  `NombreReceta` varchar(30) COLLATE utf8_spanish2_ci NOT NULL,
  `Descripcion` varchar(100) COLLATE utf8_spanish2_ci NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `ValorTotal` int(11) NOT NULL,
  `Ciudad` varchar(50) COLLATE utf8_spanish2_ci NOT NULL,
  `ImagenReceta` text COLLATE utf8_spanish2_ci DEFAULT NULL,
  PRIMARY KEY (`IdReceta`),
  KEY `FK_recetausuario` (`IdUsuario`),
  CONSTRAINT `FK_recetausuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

/*Data for the table `receta` */

/*Table structure for table `usuario` */

DROP TABLE IF EXISTS `usuario`;

CREATE TABLE `usuario` (
  `IdUsuario` int(11) NOT NULL AUTO_INCREMENT,
  `LoginUsuario` varchar(15) COLLATE utf8_spanish2_ci NOT NULL,
  `PasswordUsuario` varchar(20) COLLATE utf8_spanish2_ci NOT NULL,
  `NombreUsuario` varchar(20) COLLATE utf8_spanish2_ci NOT NULL,
  `ApellidoUsuario` varchar(20) COLLATE utf8_spanish2_ci NOT NULL,
  `DocumentoUsuario` varchar(20) COLLATE utf8_spanish2_ci NOT NULL,
  `ImagenUsuario` text COLLATE utf8_spanish2_ci DEFAULT NULL,
  `IdPerfil` int(11) NOT NULL,
  PRIMARY KEY (`IdUsuario`),
  UNIQUE KEY `UnicoUsuarioDocumento` (`LoginUsuario`,`DocumentoUsuario`),
  KEY `FK_UsuarioPerfil` (`IdPerfil`),
  CONSTRAINT `FK_UsuarioPerfil` FOREIGN KEY (`IdPerfil`) REFERENCES `perfil` (`IdPerfil`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

/*Data for the table `usuario` */

insert  into `usuario`(`IdUsuario`,`LoginUsuario`,`PasswordUsuario`,`NombreUsuario`,`ApellidoUsuario`,`DocumentoUsuario`,`ImagenUsuario`,`IdPerfil`) values (1,'rgutierrez','0912','Rosa','Gutierrez','32680584','C:\\Users\\Usuario\\Desktop\\recsys\\WebApiRecSys\\wwwroot\\Upload\\156.jpg892',2),(2,'hpaez','0912','Henry','Paez','1140855211',NULL,1),(7,'aparra','1012','Anyelis','Parra','3205669478','C:\\Users\\Usuario\\Desktop\\recsys\\WebApiRecSys\\wwwroot\\Upload\\101.webp',1),(8,'lrueda','3021','Luis','Rueda','777777777','C:\\Users\\Usuario\\Desktop\\recsys\\WebApiRecSys\\wwwroot\\Upload\\018.webp',2);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
