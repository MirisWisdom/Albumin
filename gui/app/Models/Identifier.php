<?php

namespace App\Models;

use Illuminate\Support\Facades\Request;

class Identifier
{
    /**
     * Infers the current client's semi-unique yet anonymous ID: a SHA256 hash of their IP address.
     *
     * @param string|null $ip
     * @return bool|string
     */
    public static function infer(string $ip = null): bool|string
    {
        return hash('sha256', $ip ?? Request::ip());
    }
}
