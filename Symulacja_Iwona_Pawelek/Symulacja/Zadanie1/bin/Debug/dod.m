## Copyright (C) 2017 Iwona
## 
## This program is free software; you can redistribute it and/or modify it
## under the terms of the GNU General Public License as published by
## the Free Software Foundation; either version 3 of the License, or
## (at your option) any later version.
## 
## This program is distributed in the hope that it will be useful,
## but WITHOUT ANY WARRANTY; without even the implied warranty of
## MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
## GNU General Public License for more details.
## 
## You should have received a copy of the GNU General Public License
## along with this program.  If not, see <http://www.gnu.org/licenses/>.

## -*- texinfo -*- 
## @deftypefn {} {@var{retval} =} dod (@var{input1}, @var{input2})
##
## @seealso{}
## @end deftypefn

## Author: Iwona <Iwona@DESKTOP-MN24FC4>
## Created: 2017-05-19


function Z=dod(X,Y) 
X=reshape(X,1,length(X));%to tylko by dzia³a³a na kolumnê i wiersz 
Y=reshape(Y,1,length(Y)); 
a=max(length(X),length(Y)); 
X=[X, zeros(1,a-length(X))]; 
Y=[Y,zeros(1,a-length(Y))]; 
Z=X+Y; 
endfunction
