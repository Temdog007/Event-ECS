#include "stdafx.h"
namespace {
/* code automatically generated by bin2c -- DO NOT EDIT *//* #include'ing this file in a C program is equivalent to calling  if (luaL_loadfile(L,"messageParser.lua")==0) lua_call(L, 0, 0); *//* messageParser.lua */static const unsigned char MESSAGEPARSER[]={108,111, 99, 97,108, 32, 83,121,115,116,101,109,115, 32, 61, 32,114,101,113,117,105,114,101, 40, 34,115,121,115,116,101,109, 76,105,115,116, 34, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32,102,117,110, 99,116,105,111,110, 32,112, 97,114,115,101, 70,117,110, 99,116,105,111,110, 40,108, 41, 13, 10, 32, 32, 97,115,115,101,114,116, 40,116,121,112,101, 40,108, 41, 32, 61, 61, 32, 34,115,116,114,105,110,103, 34, 44, 32, 34, 77,117,115,116, 32,112, 97,115,115, 32, 97, 32,115,116,114,105,110,103, 32,116,111, 32,112, 97,114,115,101, 34, 41, 13, 10, 32, 32,108,111, 99, 97,108, 32,109,101,115,115, 97,103,101, 32, 61, 32,115,116,114,105,110,103, 46,115,112,108,105,116, 40,108, 44, 32, 34,124, 34, 41, 13, 10, 32, 32,108,111, 99, 97,108, 32, 99,111,109,109, 97,110,100, 32, 61, 32,109,101,115,115, 97,103,101, 91, 49, 93, 13, 10, 13, 10, 32, 32,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 65,100,100, 67,111,109,112,111,110,101,110,116, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 73, 68, 32, 61, 32, 97,115,115,101,114,116, 40,116,111,110,117,109, 98,101,114, 40,109,101,115,115, 97,103,101, 91, 51, 93, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 67, 97,110,110,111,116, 32,112, 97,114,115,101, 32, 39, 37,115, 39, 32,116,111, 32,110,117,109, 98,101,114, 34, 44, 32,109,101,115,115, 97,103,101, 91, 51, 93, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32, 99,111,109,112,111,110,101,110,116, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 52, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32, 97,115,115,101,114,116, 40, 83,121,115,116,101,109,115, 46,103,101,116, 83,121,115,116,101,109, 40,115,121,115,116,101,109, 78, 97,109,101, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,115,121,115,116,101,109, 32,119,105,116,104, 32,116,104,101, 32,110, 97,109,101, 32, 39, 37,115, 39, 34, 44, 32,115,121,115,116,101,109, 78, 97,109,101, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 32, 61, 32, 97,115,115,101,114,116, 40,115,121,115,116,101,109, 58,102,105,110,100, 69,110,116,105,116,121, 40,101,110,116,105,116,121, 73, 68, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,101,110,116,105,116,121, 32,119,105,116,104, 32, 73, 68, 32, 37,100, 32,102,111,117,110,100, 34, 44, 32,101,110,116,105,116,121, 73, 68, 41, 41, 13, 10, 32, 32, 32, 32,101,110,116,105,116,121, 58, 97,100,100, 67,111,109,112,111,110,101,110,116, 40, 99,111,109,112,111,110,101,110,116, 78, 97,109,101, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 65,100,100, 69,110,116,105,116,121, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32, 97,115,115,101,114,116, 40, 83,121,115,116,101,109,115, 46,103,101,116, 83,121,115,116,101,109, 40,115,121,115,116,101,109, 78, 97,109,101, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,115,121,115,116,101,109, 32,119,105,116,104, 32,116,104,101, 32,110, 97,109,101, 32, 39, 37,115, 39, 34, 44, 32,115,121,115,116,101,109, 78, 97,109,101, 41, 41, 13, 10, 32, 32, 32, 32,115,121,115,116,101,109, 58, 99,114,101, 97,116,101, 69,110,116,105,116,121, 40, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 66,114,111, 97,100, 99, 97,115,116, 69,118,101,110,116, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 83,121,115,116,101,109,115, 46,112,117,115,104, 69,118,101,110,116, 40,109,101,115,115, 97,103,101, 91, 50, 93, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 68,105,115,112, 97,116, 99,104, 69,118,101,110,116, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32, 97,115,115,101,114,116, 40, 83,121,115,116,101,109,115, 46,103,101,116, 83,121,115,116,101,109, 40,115,121,115,116,101,109, 78, 97,109,101, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,115,121,115,116,101,109, 32,119,105,116,104, 32,116,104,101, 32,110, 97,109,101, 32, 39, 37,115, 39, 34, 44, 32,115,121,115,116,101,109, 78, 97,109,101, 41, 41, 13, 10, 32, 32, 32, 32,115,121,115,116,101,109, 58,100,105,115,112, 97,116, 99,104, 69,118,101,110,116, 40,109,101,115,115, 97,103,101, 91, 51, 93, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 68,105,115,112, 97,116, 99,104, 69,118,101,110,116, 69,110,116,105,116,121, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 73, 68, 32, 61, 32, 97,115,115,101,114,116, 40,116,111,110,117,109, 98,101,114, 40,109,101,115,115, 97,103,101, 91, 51, 93, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 67, 97,110,110,111,116, 32,112, 97,114,115,101, 32, 39, 37,115, 39, 32,116,111, 32,110,117,109, 98,101,114, 34, 44, 32,109,101,115,115, 97,103,101, 91, 51, 93, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32, 97,115,115,101,114,116, 40, 83,121,115,116,101,109,115, 46,103,101,116, 83,121,115,116,101,109, 40,115,121,115,116,101,109, 78, 97,109,101, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,115,121,115,116,101,109, 32,119,105,116,104, 32,116,104,101, 32,110, 97,109,101, 32, 39, 37,115, 39, 34, 44, 32,115,121,115,116,101,109, 78, 97,109,101, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 32, 61, 32, 97,115,115,101,114,116, 40,115,121,115,116,101,109, 58,102,105,110,100, 69,110,116,105,116,121, 40,101,110,116,105,116,121, 73, 68, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,101,110,116,105,116,121, 32,119,105,116,104, 32, 73, 68, 32, 37,100, 32,102,111,117,110,100, 34, 44, 32,101,110,116,105,116,121, 73, 68, 41, 41, 13, 10, 32, 32, 32, 32,101,110,116,105,116,121, 58,100,105,115,112, 97,116, 99,104, 69,118,101,110,116, 40,109,101,115,115, 97,103,101, 91, 52, 93, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 69,120,101, 99,117,116,101, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,102, 32, 61, 32, 97,115,115,101,114,116, 40,108,111, 97,100,115,116,114,105,110,103, 40,109,101,115,115, 97,103,101, 91, 50, 93, 41, 41, 13, 10, 32, 32, 32, 32,102, 40, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 82,101,108,111, 97,100, 77,111,100,117,108,101, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,109,111,100, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,112, 97, 99,107, 97,103,101, 46,108,111, 97,100,101,100, 91,109,111,100, 78, 97,109,101, 93, 32, 61, 32,110,105,108, 13, 10, 32, 32, 32, 32,114,101,113,117,105,114,101, 40,109,111,100, 78, 97,109,101, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 82,101,109,111,118,101, 67,111,109,112,111,110,101,110,116, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 73, 68, 32, 61, 32, 97,115,115,101,114,116, 40,116,111,110,117,109, 98,101,114, 40,109,101,115,115, 97,103,101, 91, 51, 93, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 67, 97,110,110,111,116, 32,112, 97,114,115,101, 32, 39, 37,115, 39, 32,116,111, 32,110,117,109, 98,101,114, 34, 44, 32,109,101,115,115, 97,103,101, 91, 51, 93, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32, 99,111,109,112,111,110,101,110,116, 73, 68, 32, 61, 32, 97,115,115,101,114,116, 40,116,111,110,117,109, 98,101,114, 40,109,101,115,115, 97,103,101, 91, 52, 93, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 67, 97,110,110,111,116, 32,112, 97,114,115,101, 32, 39, 37,115, 39, 32,116,111, 32,110,117,109, 98,101,114, 34, 44, 32,109,101,115,115, 97,103,101, 91, 52, 93, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32, 97,115,115,101,114,116, 40, 83,121,115,116,101,109,115, 46,103,101,116, 83,121,115,116,101,109, 40,115,121,115,116,101,109, 78, 97,109,101, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,115,121,115,116,101,109, 32,119,105,116,104, 32,116,104,101, 32,110, 97,109,101, 32, 39, 37,115, 39, 34, 44, 32,115,121,115,116,101,109, 78, 97,109,101, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 32, 61, 32, 97,115,115,101,114,116, 40,115,121,115,116,101,109, 58,102,105,110,100, 69,110,116,105,116,121, 40,101,110,116,105,116,121, 73, 68, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,101,110,116,105,116,121, 32,119,105,116,104, 32, 73, 68, 32, 37,100, 32,102,111,117,110,100, 34, 44, 32,101,110,116,105,116,121, 73, 68, 41, 41, 13, 10, 32, 32, 32, 32, 97,115,115,101,114,116, 40,101,110,116,105,116,121, 58,114,101,109,111,118,101, 67,111,109,112,111,110,101,110,116, 40, 99,111,109,112,111,110,101,110,116, 73, 68, 41, 44, 32, 34, 65, 32, 99,111,109,112,111,110,101,110,116, 32,110,111,116, 32,114,101,109,111,118,101,100, 34, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 82,101,109,111,118,101, 69,110,116,105,116,121, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 73, 68, 32, 61, 32, 97,115,115,101,114,116, 40,116,111,110,117,109, 98,101,114, 40,109,101,115,115, 97,103,101, 91, 51, 93, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 67, 97,110,110,111,116, 32,112, 97,114,115,101, 32, 39, 37,115, 39, 32,116,111, 32,110,117,109, 98,101,114, 34, 44, 32,109,101,115,115, 97,103,101, 91, 51, 93, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32, 97,115,115,101,114,116, 40, 83,121,115,116,101,109,115, 46,103,101,116, 83,121,115,116,101,109, 40,115,121,115,116,101,109, 78, 97,109,101, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,115,121,115,116,101,109, 32,119,105,116,104, 32,116,104,101, 32,110, 97,109,101, 32, 39, 37,115, 39, 34, 44, 32,115,121,115,116,101,109, 78, 97,109,101, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 32, 61, 32, 97,115,115,101,114,116, 40,115,121,115,116,101,109, 58,102,105,110,100, 69,110,116,105,116,121, 40,101,110,116,105,116,121, 73, 68, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,101,110,116,105,116,121, 32,119,105,116,104, 32, 73, 68, 32, 37,100, 32,102,111,117,110,100, 34, 44, 32,101,110,116,105,116,121, 73, 68, 41, 41, 13, 10, 32, 32, 32, 32, 97,115,115,101,114,116, 40,101,110,116,105,116,121, 58,114,101,109,111,118,101, 40, 41, 44, 32, 34, 69,110,116,105,116,121, 32,110,111,116, 32,114,101,109,111,118,101,100, 34, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 82,101,115,101,116, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111,118,101, 46,101,118,101,110,116, 46,113,117,105,116, 40, 34,114,101,115,116, 97,114,116, 34, 41, 13, 10, 13, 10, 32, 32,101,108,115,101,105,102, 32, 99,111,109,109, 97,110,100, 32, 61, 61, 32, 34, 83,101,116, 69,110,116,105,116,121, 86, 97,108,117,101, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 78, 97,109,101, 32, 61, 32,109,101,115,115, 97,103,101, 91, 50, 93, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 73, 68, 32, 61, 32, 97,115,115,101,114,116, 40,116,111,110,117,109, 98,101,114, 40,109,101,115,115, 97,103,101, 91, 51, 93, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 67, 97,110,110,111,116, 32,112, 97,114,115,101, 32, 39, 37,115, 39, 32,116,111, 32,110,117,109, 98,101,114, 34, 44, 32,109,101,115,115, 97,103,101, 91, 51, 93, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,107,101,121, 32, 61, 32, 97,115,115,101,114,116, 40,109,101,115,115, 97,103,101, 91, 52, 93, 44, 32, 34, 77,117,115,116, 32,104, 97,118,101, 32, 97, 32,107,101,121, 34, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,118, 97,108,117,101, 32, 61, 32, 97,115,115,101,114,116, 40,109,101,115,115, 97,103,101, 91, 53, 93, 44, 32, 34, 77,117,115,116, 32,104, 97,118,101, 32, 97, 32,118, 97,108,117,101, 34, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32, 97,115,115,101,114,116, 40, 83,121,115,116,101,109,115, 46,103,101,116, 83,121,115,116,101,109, 40,115,121,115,116,101,109, 78, 97,109,101, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,115,121,115,116,101,109, 32,119,105,116,104, 32,116,104,101, 32,110, 97,109,101, 32, 39, 37,115, 39, 34, 44, 32,115,121,115,116,101,109, 78, 97,109,101, 41, 41, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,110,116,105,116,121, 32, 61, 32, 97,115,115,101,114,116, 40,115,121,115,116,101,109, 58,102,105,110,100, 69,110,116,105,116,121, 40,101,110,116,105,116,121, 73, 68, 41, 44, 32,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 78,111, 32,101,110,116,105,116,121, 32,119,105,116,104, 32, 73, 68, 32, 37,100, 32,102,111,117,110,100, 34, 44, 32,101,110,116,105,116,121, 73, 68, 41, 41, 13, 10, 13, 10, 32, 32, 32, 32,105,102, 32,116,111,110,117,109, 98,101,114, 40,107,101,121, 41, 32,116,104,101,110, 32,107,101,121, 32, 61, 32,116,111,110,117,109, 98,101,114, 40,107,101,121, 41, 32,101,110,100, 13, 10, 32, 32, 32, 32, 13, 10, 32, 32, 32, 32,105,102, 32,116,111,110,117,109, 98,101,114, 40,118, 97,108,117,101, 41, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,101,110,116,105,116,121, 58,115,101,116, 40,107,101,121, 44, 32,116,111,110,117,109, 98,101,114, 40,118, 97,108,117,101, 41, 41, 13, 10, 32, 32, 32, 32,101,108,115,101,105,102, 32,107,101,121, 32, 61, 61, 32, 34,101,110, 97, 98,108,101,100, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,101,110,116,105,116,121, 58,115,101,116, 69,110, 97, 98,108,101,100, 40,118, 97,108,117,101, 58,108,111,119,101,114, 40, 41, 32, 61, 61, 32, 34,116,114,117,101, 34, 41, 13, 10, 32, 32, 32, 32,101,108,115,101,105,102, 32,118, 97,108,117,101, 58,108,111,119,101,114, 40, 41, 32, 61, 61, 32, 34,116,114,117,101, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,101,110,116,105,116,121, 58,115,101,116, 40,107,101,121, 44, 32,116,114,117,101, 41, 13, 10, 32, 32, 32, 32,101,108,115,101,105,102, 32,118, 97,108,117,101, 58,108,111,119,101,114, 40, 41, 32, 61, 61, 32, 34,102, 97,108,115,101, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,101,110,116,105,116,121, 58,115,101,116, 40,107,101,121, 44, 32,102, 97,108,115,101, 41, 13, 10, 32, 32, 32, 32,101,108,115,101, 13, 10, 32, 32, 32, 32, 32, 32,101,110,116,105,116,121, 58,115,101,116, 40,107,101,121, 44, 32,118, 97,108,117,101, 41, 13, 10, 32, 32, 32, 32,101,110,100, 13, 10, 13, 10, 32, 32,101,108,115,101, 13, 10, 32, 32, 32, 32,112,114,105,110,116, 40,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 85,110,107,110,111,119,110, 32, 99,111,109,109, 97,110,100, 32, 39, 37,115, 39, 32,114,101, 99,101,105,118,101,100, 34, 44, 32, 99,111,109,109, 97,110,100, 41, 41, 13, 10, 32, 32,101,110,100, 13, 10,101,110,100, 13, 10, 13, 10,114,101,116,117,114,110, 32,115,101,116,109,101,116, 97,116, 97, 98,108,101, 40,123,125, 44, 13, 10,123, 13, 10, 32, 32, 95, 95, 99, 97,108,108, 32, 61, 32,102,117,110, 99,116,105,111,110, 40,115, 44, 32,108, 41, 13, 10, 32, 32, 32, 32,114,101,116,117,114,110, 32,112, 97,114,115,101, 70,117,110, 99,116,105,111,110, 40,108, 41, 13, 10, 32, 32,101,110,100, 13, 10,125, 41, 13, 10,};int luaopen_messageParser(lua_State* L)
{
  if (luaL_loadbuffer(L,(const char*)MESSAGEPARSER,sizeof(MESSAGEPARSER),"messageParser.lua")==0) lua_call(L, 0, 1);  return 1;
}
/* code automatically generated by bin2c -- DO NOT EDIT *//* #include'ing this file in a C program is equivalent to calling  if (luaL_loadfile(L,"ServerComponent.lua")==0) lua_call(L, 0, 0); *//* ServerComponent.lua */static const unsigned char SERVERCOMPONENT[]={ 45, 45, 32, 67,111,112,121,114,105,103,104,116, 32, 40, 99, 41, 32, 50, 48, 49, 56, 32, 84,101,109,100,111,103, 48, 48, 55, 13, 10, 45, 45, 13, 10, 45, 45, 32, 80,101,114,109,105,115,115,105,111,110, 32,105,115, 32,104,101,114,101, 98,121, 32,103,114, 97,110,116,101,100, 44, 32,102,114,101,101, 32,111,102, 32, 99,104, 97,114,103,101, 44, 32,116,111, 32, 97,110,121, 32,112,101,114,115,111,110, 32,111, 98,116, 97,105,110,105,110,103, 32, 97, 32, 99,111,112,121, 13, 10, 45, 45, 32,111,102, 32,116,104,105,115, 32,115,111,102,116,119, 97,114,101, 32, 97,110,100, 32, 97,115,115,111, 99,105, 97,116,101,100, 32,100,111, 99,117,109,101,110,116, 97,116,105,111,110, 32,102,105,108,101,115, 32, 40,116,104,101, 32, 34, 83,111,102,116,119, 97,114,101, 34, 41, 44, 32,116,111, 32,100,101, 97,108, 13, 10, 45, 45, 32,105,110, 32,116,104,101, 32, 83,111,102,116,119, 97,114,101, 32,119,105,116,104,111,117,116, 32,114,101,115,116,114,105, 99,116,105,111,110, 44, 32,105,110, 99,108,117,100,105,110,103, 32,119,105,116,104,111,117,116, 32,108,105,109,105,116, 97,116,105,111,110, 32,116,104,101, 32,114,105,103,104,116,115, 13, 10, 45, 45, 32,116,111, 32,117,115,101, 44, 32, 99,111,112,121, 44, 32,109,111,100,105,102,121, 44, 32,109,101,114,103,101, 44, 32,112,117, 98,108,105,115,104, 44, 32,100,105,115,116,114,105, 98,117,116,101, 44, 32,115,117, 98,108,105, 99,101,110,115,101, 44, 32, 97,110,100, 47,111,114, 32,115,101,108,108, 13, 10, 45, 45, 32, 99,111,112,105,101,115, 32,111,102, 32,116,104,101, 32, 83,111,102,116,119, 97,114,101, 44, 32, 97,110,100, 32,116,111, 32,112,101,114,109,105,116, 32,112,101,114,115,111,110,115, 32,116,111, 32,119,104,111,109, 32,116,104,101, 32, 83,111,102,116,119, 97,114,101, 32,105,115, 13, 10, 45, 45, 32,102,117,114,110,105,115,104,101,100, 32,116,111, 32,100,111, 32,115,111, 44, 32,115,117, 98,106,101, 99,116, 32,116,111, 32,116,104,101, 32,102,111,108,108,111,119,105,110,103, 32, 99,111,110,100,105,116,105,111,110,115, 58, 13, 10, 45, 45, 13, 10, 45, 45, 32, 84,104,101, 32, 97, 98,111,118,101, 32, 99,111,112,121,114,105,103,104,116, 32,110,111,116,105, 99,101, 32, 97,110,100, 32,116,104,105,115, 32,112,101,114,109,105,115,115,105,111,110, 32,110,111,116,105, 99,101, 32,115,104, 97,108,108, 32, 98,101, 32,105,110, 99,108,117,100,101,100, 32,105,110, 32, 97,108,108, 13, 10, 45, 45, 32, 99,111,112,105,101,115, 32,111,114, 32,115,117, 98,115,116, 97,110,116,105, 97,108, 32,112,111,114,116,105,111,110,115, 32,111,102, 32,116,104,101, 32, 83,111,102,116,119, 97,114,101, 46, 13, 10, 45, 45, 13, 10, 45, 45, 32, 84, 72, 69, 32, 83, 79, 70, 84, 87, 65, 82, 69, 32, 73, 83, 32, 80, 82, 79, 86, 73, 68, 69, 68, 32, 34, 65, 83, 32, 73, 83, 34, 44, 32, 87, 73, 84, 72, 79, 85, 84, 32, 87, 65, 82, 82, 65, 78, 84, 89, 32, 79, 70, 32, 65, 78, 89, 32, 75, 73, 78, 68, 44, 32, 69, 88, 80, 82, 69, 83, 83, 32, 79, 82, 13, 10, 45, 45, 32, 73, 77, 80, 76, 73, 69, 68, 44, 32, 73, 78, 67, 76, 85, 68, 73, 78, 71, 32, 66, 85, 84, 32, 78, 79, 84, 32, 76, 73, 77, 73, 84, 69, 68, 32, 84, 79, 32, 84, 72, 69, 32, 87, 65, 82, 82, 65, 78, 84, 73, 69, 83, 32, 79, 70, 32, 77, 69, 82, 67, 72, 65, 78, 84, 65, 66, 73, 76, 73, 84, 89, 44, 13, 10, 45, 45, 32, 70, 73, 84, 78, 69, 83, 83, 32, 70, 79, 82, 32, 65, 32, 80, 65, 82, 84, 73, 67, 85, 76, 65, 82, 32, 80, 85, 82, 80, 79, 83, 69, 32, 65, 78, 68, 32, 78, 79, 78, 73, 78, 70, 82, 73, 78, 71, 69, 77, 69, 78, 84, 46, 32, 73, 78, 32, 78, 79, 32, 69, 86, 69, 78, 84, 32, 83, 72, 65, 76, 76, 32, 84, 72, 69, 13, 10, 45, 45, 32, 65, 85, 84, 72, 79, 82, 83, 32, 79, 82, 32, 67, 79, 80, 89, 82, 73, 71, 72, 84, 32, 72, 79, 76, 68, 69, 82, 83, 32, 66, 69, 32, 76, 73, 65, 66, 76, 69, 32, 70, 79, 82, 32, 65, 78, 89, 32, 67, 76, 65, 73, 77, 44, 32, 68, 65, 77, 65, 71, 69, 83, 32, 79, 82, 32, 79, 84, 72, 69, 82, 13, 10, 45, 45, 32, 76, 73, 65, 66, 73, 76, 73, 84, 89, 44, 32, 87, 72, 69, 84, 72, 69, 82, 32, 73, 78, 32, 65, 78, 32, 65, 67, 84, 73, 79, 78, 32, 79, 70, 32, 67, 79, 78, 84, 82, 65, 67, 84, 44, 32, 84, 79, 82, 84, 32, 79, 82, 32, 79, 84, 72, 69, 82, 87, 73, 83, 69, 44, 32, 65, 82, 73, 83, 73, 78, 71, 32, 70, 82, 79, 77, 44, 13, 10, 45, 45, 32, 79, 85, 84, 32, 79, 70, 32, 79, 82, 32, 73, 78, 32, 67, 79, 78, 78, 69, 67, 84, 73, 79, 78, 32, 87, 73, 84, 72, 32, 84, 72, 69, 32, 83, 79, 70, 84, 87, 65, 82, 69, 32, 79, 82, 32, 84, 72, 69, 32, 85, 83, 69, 32, 79, 82, 32, 79, 84, 72, 69, 82, 32, 68, 69, 65, 76, 73, 78, 71, 83, 32, 73, 78, 32, 84, 72, 69, 13, 10, 45, 45, 32, 83, 79, 70, 84, 87, 65, 82, 69, 46, 13, 10, 13, 10,108,111, 99, 97,108, 32,112, 97,114,115,101,114, 32, 61, 32,114,101,113,117,105,114,101, 40, 34,109,101,115,115, 97,103,101, 80, 97,114,115,101,114, 34, 41, 13, 10,108,111, 99, 97,108, 32, 67,111,109,112,111,110,101,110,116, 32, 61, 32,114,101,113,117,105,114,101, 40, 39, 99,111,109,112,111,110,101,110,116, 39, 41, 13, 10,108,111, 99, 97,108, 32, 99,108, 97,115,115, 32, 61, 32,114,101,113,117,105,114,101, 40, 39, 99,108, 97,115,115,108,105, 98, 39, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32, 83,121,115,116,101,109,115, 32, 61, 32,114,101,113,117,105,114,101, 40, 34,115,121,115,116,101,109, 76,105,115,116, 34, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32,115,111, 99,107,101,116, 32, 61, 32,114,101,113,117,105,114,101, 40, 34,115,111, 99,107,101,116, 34, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 32, 61, 32, 99,108, 97,115,115, 40, 39,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 39, 44, 32, 67,111,109,112,111,110,101,110,116, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32, 99,111,110,115,111,108,101, 80,114,105,110,116, 32, 61, 32,112,114,105,110,116, 13, 10, 13, 10,108,111, 99, 97,108, 32,101,111,115, 32, 61, 32,115,116,114,105,110,103, 46, 99,104, 97,114, 40, 51, 41, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58, 95, 95,105,110,105,116, 40, 41, 13, 10, 32, 32,115,101,108,102, 46,104,111,115,116, 32, 61, 32, 34, 42, 34, 13, 10, 32, 32,115,101,108,102, 46,112,111,114,116, 32, 61, 32, 51, 50, 52, 56, 53, 13, 10, 32, 32,115,101,108,102, 46,116,105,109,101,111,117,116, 32, 61, 32, 48, 13, 10, 13, 10, 32, 32,115,101,108,102, 46, 99,117,114,114,101,110,116, 32, 61, 32, 48, 13, 10, 32, 32,115,101,108,102, 46,114, 97,116,101, 32, 61, 32, 49, 13, 10,101,110,100, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58, 99,108,111,115,101, 40, 41, 13, 10, 32, 32,105,102, 32,115,101,108,102, 46, 99,108,105,101,110,116, 32,116,104,101,110, 32,115,101,108,102, 46, 99,108,105,101,110,116, 58, 99,108,111,115,101, 40, 41, 32,101,110,100, 13, 10, 32, 32,105,102, 32,115,101,108,102, 46,115,101,114,118,101,114, 32,116,104,101,110, 32,115,101,108,102, 46,115,101,114,118,101,114, 58, 99,108,111,115,101, 40, 41, 32,101,110,100, 13, 10,101,110,100, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58, 99,111,110,110,101, 99,116, 40, 41, 13, 10, 32, 32,115,101,108,102, 58, 99,108,111,115,101, 40, 41, 13, 10, 13, 10, 32, 32,115,101,108,102, 46, 99,108,105,101,110,116, 32, 61, 32,110,105,108, 13, 10, 32, 32,115,101,108,102, 46,109,101,115,115, 97,103,101,115, 32, 61, 32,123,125, 13, 10, 13, 10, 32, 32,115,101,108,102, 46,115,101,114,118,101,114, 32, 61, 32, 97,115,115,101,114,116, 40,115,111, 99,107,101,116, 46, 98,105,110,100, 40,115,101,108,102, 46,104,111,115,116, 44, 32,115,101,108,102, 46,112,111,114,116, 41, 41, 13, 10, 32, 32,115,101,108,102, 46,115,101,114,118,101,114, 58,115,101,116,116,105,109,101,111,117,116, 40,115,101,108,102, 46,116,105,109,101,111,117,116, 41, 13, 10, 13, 10, 32, 32,108,111, 99, 97,108, 32,105, 44, 32,112, 32, 61, 32,115,101,108,102, 46,115,101,114,118,101,114, 58,103,101,116,115,111, 99,107,110, 97,109,101, 40, 41, 13, 10, 32, 32, 97,115,115,101,114,116, 40,105, 44,112, 41, 13, 10, 13, 10, 32, 32,112,114,105,110,116, 32, 61, 32,102,117,110, 99,116,105,111,110, 40, 46, 46, 46, 41, 13, 10, 32, 32, 32, 32, 99,111,110,115,111,108,101, 80,114,105,110,116, 40, 46, 46, 46, 41, 13, 10, 32, 32, 32, 32,115,101,108,102, 58,101,110,113,117,101,117,101, 77,101,115,115, 97,103,101,115, 40, 46, 46, 46, 41, 13, 10, 32, 32,101,110,100, 13, 10, 13, 10, 32, 32,112,114,105,110,116, 40,115,116,114,105,110,103, 46,102,111,114,109, 97,116, 40, 34, 83,116, 97,114,116,105,110,103, 32,115,101,114,118,101,114, 32,123, 37,115, 58, 37,117,125, 34, 44, 32,115,101,108,102, 46,104,111,115,116, 44, 32,115,101,108,102, 46,112,111,114,116, 41, 41, 13, 10,101,110,100, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58,101,110,113,117,101,117,101, 77,101,115,115, 97,103,101,115, 40, 46, 46, 46, 41, 13, 10, 32, 32,102,111,114, 32, 95, 44, 32,109, 32,105,110, 32,112, 97,105,114,115, 40,123, 46, 46, 46,125, 41, 32,100,111, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,109,101,115,115, 97,103,101, 32, 61, 32,116,111,115,116,114,105,110,103, 40,109, 41, 13, 10, 32, 32, 32, 32,105,102, 32,109,101,115,115, 97,103,101, 58,101,110,100,115, 40,101,111,115, 41, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,116, 97, 98,108,101, 46,105,110,115,101,114,116, 40,115,101,108,102, 46,109,101,115,115, 97,103,101,115, 44, 32,109,101,115,115, 97,103,101, 41, 13, 10, 32, 32, 32, 32,101,108,115,101, 13, 10, 32, 32, 32, 32, 32, 32,116, 97, 98,108,101, 46,105,110,115,101,114,116, 40,115,101,108,102, 46,109,101,115,115, 97,103,101,115, 44, 32,109,101,115,115, 97,103,101, 46, 46,101,111,115, 41, 13, 10, 32, 32, 32, 32,101,110,100, 13, 10, 32, 32,101,110,100, 13, 10,101,110,100, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58,101,118,101,110,116, 65,100,100,101,100, 67,111,109,112,111,110,101,110,116, 40, 97,114,103,115, 41, 13, 10, 32, 32,105,102, 32,110,111,116, 32, 97,114,103,115, 32,116,104,101,110, 32,114,101,116,117,114,110, 32,101,110,100, 13, 10, 32, 32,105,102, 32, 97,114,103,115, 46, 99,111,109,112,111,110,101,110,116, 32,126, 61, 32,115,101,108,102, 32,116,104,101,110, 32,114,101,116,117,114,110, 32,101,110,100, 13, 10, 13, 10, 32, 32,115,101,108,102, 58, 99,111,110,110,101, 99,116, 40, 41, 13, 10,101,110,100, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58,101,118,101,110,116, 85,112,100, 97,116,101, 83,111, 99,107,101,116, 40, 97,114,103,115, 41, 13, 10, 32, 32, 97,114,103,115, 32, 61, 32, 97,114,103,115, 32,111,114, 32,123,125, 13, 10, 13, 10, 32, 32,115,101,108,102, 46,104,111,115,116, 32, 61, 32, 97,114,103,115, 46,104,111,115,116, 32,111,114, 32,115,101,108,102, 46,104,111,115,116, 13, 10, 32, 32,115,101,108,102, 46,112,111,114,116, 32, 61, 32, 97,114,103,115, 46,112,111,114,116, 32,111,114, 32,115,101,108,102, 46,112,111,114,116, 13, 10, 32, 32,115,101,108,102, 46,116,105,109,101,111,117,116, 32, 61, 32, 97,114,103,115, 46,116,105,109,101,111,117,116, 32,111,114, 32,115,101,108,102, 46,116,105,109,101,111,117,116, 13, 10, 13, 10, 32, 32,115,101,108,102, 58, 99,111,110,110,101, 99,116, 40, 41, 13, 10,101,110,100, 13, 10, 13, 10,108,111, 99, 97,108, 32,102,117,110, 99,116,105,111,110, 32,115,101,114,105, 97,108,105,122,101, 40,115,121,115,116,101,109, 41, 13, 10, 32, 32,114,101,116,117,114,110, 32,115,121,115,116,101,109, 58,115,101,114,105, 97,108,105,122,101, 40, 41, 13, 10,101,110,100, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58,101,118,101,110,116, 85,112,100, 97,116,101, 40, 97,114,103,115, 41, 13, 10, 13, 10, 32, 32,105,102, 32,110,111,116, 32,115,101,108,102, 46, 99,108,105,101,110,116, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,101,114,114, 13, 10, 32, 32, 32, 32,115,101,108,102, 46, 99,108,105,101,110,116, 44, 32,101,114,114, 32, 61, 32,115,101,108,102, 46,115,101,114,118,101,114, 58, 97, 99, 99,101,112,116, 40, 41, 13, 10, 32, 32, 32, 32,105,102, 32,115,101,108,102, 46, 99,108,105,101,110,116, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,115,101,108,102, 46, 99,108,105,101,110,116, 58,115,101,116,116,105,109,101,111,117,116, 40,115,101,108,102, 46,116,105,109,101,111,117,116, 41, 13, 10, 32, 32, 32, 32, 32, 32,112,114,105,110,116, 40, 34, 70,111,117,110,100, 32, 99,108,105,101,110,116, 34, 41, 13, 10, 32, 32, 32, 32,101,108,115,101, 13, 10, 32, 32, 32, 32, 32, 32,105,102, 32,101,114,114, 32,126, 61, 32, 34,116,105,109,101,111,117,116, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32, 32, 32,112,114,105,110,116, 40,101,114,114, 41, 13, 10, 32, 32, 32, 32, 32, 32,101,110,100, 13, 10, 32, 32, 32, 32,101,110,100, 13, 10, 32, 32,101,110,100, 13, 10, 13, 10, 32, 32,105,102, 32,115,101,108,102, 46, 99,108,105,101,110,116, 32,116,104,101,110, 13, 10, 32, 32, 32, 32,102,111,114, 32,105, 44, 32,109,101,115,115, 97,103,101, 32,105,110, 32,112, 97,105,114,115, 40,115,101,108,102, 46,109,101,115,115, 97,103,101,115, 41, 32,100,111, 13, 10, 32, 32, 32, 32, 32, 32,108,111, 99, 97,108, 32,115,116, 97,116,117,115, 44, 32,101,114,114, 32, 61, 32,112, 99, 97,108,108, 40,115,101,108,102, 46, 99,108,105,101,110,116, 46,115,101,110,100, 44, 32,115,101,108,102, 46, 99,108,105,101,110,116, 44, 32,109,101,115,115, 97,103,101, 41, 13, 10, 32, 32, 32, 32, 32, 32,105,102, 32,110,111,116, 32,115,116, 97,116,117,115, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32, 32, 32,112,114,105,110,116, 40,101,114,114, 41, 13, 10, 32, 32, 32, 32, 32, 32, 32, 32,115,101,108,102, 46, 99,108,105,101,110,116, 32, 61, 32,110,105,108, 13, 10, 32, 32, 32, 32, 32, 32, 32, 32,114,101,116,117,114,110, 13, 10, 32, 32, 32, 32, 32, 32,101,110,100, 13, 10, 32, 32, 32, 32, 32, 32,115,101,108,102, 46,109,101,115,115, 97,103,101,115, 91,105, 93, 32, 61, 32,110,105,108, 13, 10, 32, 32, 32, 32,101,110,100, 13, 10, 13, 10, 32, 32, 32, 32,108,111, 99, 97,108, 32,108, 44,101, 32, 61, 32,115,101,108,102, 46, 99,108,105,101,110,116, 58,114,101, 99,101,105,118,101, 40, 41, 13, 10, 32, 32, 32, 32,105,102, 32,108, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,112, 97,114,115,101,114, 40,108, 41, 13, 10, 32, 32, 32, 32,101,108,115,101, 13, 10, 32, 32, 32, 32, 32, 32,105,102, 32,101, 32,126, 61, 32, 34,116,105,109,101,111,117,116, 34, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32, 32, 32,112,114,105,110,116, 40,101, 41, 13, 10, 32, 32, 32, 32, 32, 32,101,110,100, 13, 10, 32, 32, 32, 32,101,110,100, 13, 10, 13, 10, 32, 32, 32, 32,105,102, 32,110,111,116, 32, 97,114,103,115, 32,111,114, 32,110,111,116, 32, 97,114,103,115, 46,100,116, 32,116,104,101,110, 32,114,101,116,117,114,110, 32,101,110,100, 13, 10, 13, 10, 32, 32, 32, 32,115,101,108,102, 46, 99,117,114,114,101,110,116, 32, 61, 32,115,101,108,102, 46, 99,117,114,114,101,110,116, 32, 43, 32, 97,114,103,115, 46,100,116, 13, 10, 32, 32, 32, 32,105,102, 32,115,101,108,102, 46, 99,117,114,114,101,110,116, 32, 62, 32,115,101,108,102, 46,114, 97,116,101, 32,116,104,101,110, 13, 10, 32, 32, 32, 32, 32, 32,115,101,108,102, 58,101,110,113,117,101,117,101, 77,101,115,115, 97,103,101,115, 40, 83,121,115,116,101,109,115, 46,102,111,114, 69, 97, 99,104, 83,121,115,116,101,109, 40,115,101,114,105, 97,108,105,122,101, 41, 41, 13, 10, 32, 32, 32, 32, 32, 32,115,101,108,102, 46, 99,117,114,114,101,110,116, 32, 61, 32, 48, 13, 10, 32, 32, 32, 32,101,110,100, 13, 10, 32, 32,101,110,100, 13, 10,101,110,100, 13, 10, 13, 10,102,117,110, 99,116,105,111,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 58,101,118,101,110,116, 81,117,105,116, 40, 97,114,103,115, 41, 13, 10, 32, 32,115,101,108,102, 58, 99,108,111,115,101, 40, 41, 13, 10,101,110,100, 13, 10, 13, 10,114,101,116,117,114,110, 32,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 13, 10,};int luaopen_ServerComponent(lua_State* L)
{
  if (luaL_loadbuffer(L,(const char*)SERVERCOMPONENT,sizeof(SERVERCOMPONENT),"ServerComponent.lua")==0) lua_call(L, 0, 1);  return 1;
}
/* code automatically generated by bin2c -- DO NOT EDIT *//* #include'ing this file in a C program is equivalent to calling  if (luaL_loadfile(L,"serverSystem.lua")==0) lua_call(L, 0, 0); *//* serverSystem.lua */static const unsigned char SERVERSYSTEM[]={114,101,113,117,105,114,101, 40, 39,101,118,101,110,116,101, 99,115, 39, 41, 13, 10,114,101,113,117,105,114,101, 40, 39,101,118,101,110,116,101, 99,115,115,101,114,118,101,114, 39, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32, 83,101,114,118,101,114, 32, 61, 32,114,101,113,117,105,114,101, 40, 34,115,101,114,118,101,114, 67,111,109,112,111,110,101,110,116, 34, 41, 13, 10,108,111, 99, 97,108, 32,115,121,115,116,101,109,115, 32, 61, 32,114,101,113,117,105,114,101, 40, 34,115,121,115,116,101,109, 76,105,115,116, 34, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32, 83,121,115,116,101,109, 32, 61, 32,114,101,113,117,105,114,101, 40, 68, 69, 66, 85, 71, 95, 77, 79, 68, 69, 32, 97,110,100, 32, 34,100,101, 98,117,103, 83,121,115,116,101,109, 34, 32,111,114, 32, 34,115,121,115,116,101,109, 34, 41, 13, 10, 13, 10,108,111, 99, 97,108, 32,115,121,115,116,101,109, 32, 61, 32,115,121,115,116,101,109,115, 46, 97,100,100, 83,121,115,116,101,109, 40, 83,121,115,116,101,109, 40, 34, 83,101,114,118,101,114, 32, 83,121,115,116,101,109, 34, 41, 41, 13, 10,108,111, 99, 97,108, 32,101,110, 32, 61, 32,115,121,115,116,101,109, 58, 99,114,101, 97,116,101, 69,110,116,105,116,121, 40, 41, 13, 10,101,110, 46,110, 97,109,101, 32, 61, 32, 34, 83,121,115,116,101,109, 32, 69,110,116,105,116,121, 34, 13, 10,101,110, 58, 97,100,100, 67,111,109,112,111,110,101,110,116, 40, 83,101,114,118,101,114, 41, 13, 10,101,110, 58, 97,100,100, 67,111,109,112,111,110,101,110,116, 40, 34,102,105,110, 97,108,105,122,101,114, 67,111,109,112,111,110,101,110,116, 34, 41, 13, 10,};int luaopen_serverSystem(lua_State* L)
{
  if (luaL_loadbuffer(L,(const char*)SERVERSYSTEM,sizeof(SERVERSYSTEM),"serverSystem.lua")==0) lua_call(L, 0, 1);  return 1;
}

const std::map<const char*, lua_CFunction> funcMap =
{

	{"messageParser", luaopen_messageParser},
	{"ServerComponent", luaopen_ServerComponent},
	{"serverSystem", luaopen_serverSystem},
};
}
